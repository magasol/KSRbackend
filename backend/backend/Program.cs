using backend.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            {
                var loginThread = Task.Factory.StartNew(() =>
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "loginQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        var loginConsumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: "loginQueue", autoAck: false, consumer: loginConsumer);
                        channel.BasicQos(0, 1, false);

                        loginConsumer.Received += AddReceiver(channel, LoginService.PrepareLoginResponse, LoginService.PrepareEmptyLoginRepsonse);
                        Console.WriteLine(" loginThread Awaiting RPC requests");

                        Console.ReadLine();
                    }
                });

                var registerThread = Task.Factory.StartNew(() =>
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "registerQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        var registerConsumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: "registerQueue", autoAck: false, consumer: registerConsumer);
                        channel.BasicQos(0, 1, false);

                        registerConsumer.Received += AddReceiver(channel, RegisterService.PrepareRegisterResponse, RegisterService.PrepareEmptyRegisterRepsonse);
                        Console.WriteLine(" registerThread Awaiting RPC requests");

                        Console.ReadLine();
                    }
                });

                var searchThread = Task.Factory.StartNew(() =>
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "searchQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        var searchConsumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: "searchQueue", autoAck: false, consumer: searchConsumer);
                        channel.BasicQos(0, 1, false);

                        searchConsumer.Received += AddReceiver(channel, SearchService.PrepareSearchResponse, SearchService.PrepareEmptySearchRepsonse);
                        Console.WriteLine(" searchThread Awaiting RPC requests");

                        Console.ReadLine();
                    }
                });

                var buyThread = Task.Factory.StartNew(() =>
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "buyQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        var buyConsumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: "buyQueue", autoAck: false, consumer: buyConsumer);
                        channel.BasicQos(0, 1, false);

                        buyConsumer.Received += AddReceiver(channel, BuyService.PrepareBuyResponse, BuyService.PrepareEmptyBuyRepsonse);
                        Console.WriteLine(" buyThread Awaiting RPC requests");

                        Console.ReadLine();
                    }
                });

                var ticketsThread = Task.Factory.StartNew(() =>
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "ticketsQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        var ticketsConsumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: "ticketsQueue", autoAck: false, consumer: ticketsConsumer);
                        channel.BasicQos(0, 1, false);

                        ticketsConsumer.Received += AddReceiver(channel, TicketService.PrepareTicketResponse, TicketService.PrepareEmptyTicketRepsonse);
                        Console.WriteLine(" ticketsThread Awaiting RPC requests");

                        Console.ReadLine();
                    }
                });

                Task.WaitAll(loginThread, registerThread, searchThread, buyThread, ticketsThread);

                var work = new ConcurrentQueue<EventingBasicConsumer>();

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