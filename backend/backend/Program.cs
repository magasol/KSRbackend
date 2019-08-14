﻿using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace backend
{
    class Program
    {
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

                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "loginQueue", autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        string[] travellerData = message.Split(',');
                        Console.WriteLine(" Login credentials: ({0})", message);

                        response = PrepareResponse(travellerData[0], travellerData[1]);
                        Console.WriteLine(" response ({0})", response);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
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

        private static string PrepareResponse(string login, string password)
        {
            TravellerRepository travellerRepository = new TravellerRepository();
            Traveller traveller = travellerRepository.FindUserByLogin(login);
            LoginResponse loginResponse;
            if(traveller.password == password)
            {
                loginResponse = new LoginResponse(true,
                    traveller.id, traveller.first_name, traveller.last_name, traveller.email, traveller.login);
            }
            else
            {
                loginResponse = new LoginResponse(false);
            }
            return loginResponse.ToString();
        }
    }
}
