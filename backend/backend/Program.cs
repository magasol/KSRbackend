using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace backend
{
    class Program
    {
        internal static RegisterService RegisterService => RegisterService;
        internal static LoginService LoginService => LoginService;

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "loginQueue",
                    durable: false, 
                    exclusive: false,
                    autoDelete: false, 
                    arguments: null);

                channel.QueueDeclare(
                    queue: "registerQueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.BasicQos(0, 1, false);

                var loginConsumer = new EventingBasicConsumer(channel);
                var registerConsumer = new EventingBasicConsumer(channel);

                channel.BasicConsume(queue: "loginQueue", autoAck: false, consumer: loginConsumer);
                channel.BasicConsume(queue: "registerQueue", autoAck: false, consumer: registerConsumer);

                Console.WriteLine(" [x] Awaiting RPC requests");

                loginConsumer.Received += (model, ea) =>
                {
                    string response = null;
                    
                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        response = LoginService.PrepareLoginResponse(message);
                        Console.WriteLine(" response ({0})", response);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = new UserDataResponse(false).ToString();
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(
                            exchange: "",
                            routingKey: props.ReplyTo,
                            basicProperties: replyProps,
                            body: responseBytes);

                        channel.BasicAck(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false);
                    }
                };

                registerConsumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        response = RegisterService.PrepareRegisterResponse(message);
                        Console.WriteLine(" response ({0})", response);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = new UserDataResponse(false).ToString();
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(
                            exchange: "",
                            routingKey: props.ReplyTo,
                            basicProperties: replyProps,
                            body: responseBytes);

                        channel.BasicAck(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }        
    }
}
