using backend.Services;
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
        internal static SearchService SearchService => SearchService;
        internal static BuyService BuyService => BuyService;
        internal static TicketService TicketService => TicketService;

        public delegate string prepareResponseFunction(string message);
        public delegate string prepareEmptyResponseFunction();

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "loginQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.QueueDeclare( queue: "registerQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.QueueDeclare( queue: "searchQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.QueueDeclare( queue: "buyQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.QueueDeclare(queue: "ticketsQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(0, 1, false);

                var loginConsumer = new EventingBasicConsumer(channel);
                var registerConsumer = new EventingBasicConsumer(channel);
                var searchConsumer = new EventingBasicConsumer(channel);
                var buyConsumer = new EventingBasicConsumer(channel);
                var ticketsConsumer = new EventingBasicConsumer(channel);

                channel.BasicConsume(queue: "loginQueue", autoAck: false, consumer: loginConsumer);
                channel.BasicConsume(queue: "registerQueue", autoAck: false, consumer: registerConsumer);
                channel.BasicConsume(queue: "searchQueue", autoAck: false, consumer: searchConsumer);
                channel.BasicConsume(queue: "buyQueue", autoAck: false, consumer: buyConsumer);
                channel.BasicConsume(queue: "ticketsQueue", autoAck: false, consumer: ticketsConsumer);

                Console.WriteLine(" [x] Awaiting RPC requests");

                loginConsumer.Received += AddReceiver(channel, LoginService.PrepareLoginResponse, LoginService.PrepareEmptyLoginRepsonse);

                registerConsumer.Received += AddReceiver(channel, RegisterService.PrepareRegisterResponse, RegisterService.PrepareEmptyRegisterRepsonse);
                
                searchConsumer.Received += AddReceiver(channel, SearchService.PrepareSearchResponse, SearchService.PrepareEmptySearchRepsonse);

                buyConsumer.Received += AddReceiver(channel, BuyService.PrepareBuyResponse, BuyService.PrepareEmptyBuyRepsonse);

                ticketsConsumer.Received += AddReceiver(channel, TicketService.PrepareTicketResponse, TicketService.PrepareEmptyTicketRepsonse);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static EventHandler<BasicDeliverEventArgs> AddReceiver(IModel channel, prepareResponseFunction prepareResponse,
           prepareEmptyResponseFunction prepareEmptyResponse)
        {
            return (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        response = prepareResponse(message);
                        Console.WriteLine(" response ({0})", response);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = prepareEmptyResponse();
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
        }
    }
}
