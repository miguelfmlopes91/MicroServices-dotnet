using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["rabbitmq-clusterip-srv"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShuwDown;
                
                Console.WriteLine("--> Connected to Message Bus");
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {e.Message}"); 
            }
        }

        private void RabbitMQ_ConnectionShuwDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void PublishNewPlatform(PlatformPublishedDto publishedDto)
        {
            var message = JsonSerializer.Serialize(publishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("---> RabbitMQ Connection Open, sending message ...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("---> RabbitMQ Connection Closed, not sending message ...");

            }
        }

        public void Dispose()
        {
            Console.WriteLine("Message Bus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange:"trigger", 
                routingKey:"",
                basicProperties: null,
                body: body
                );
            Console.WriteLine($"---> We have sent message {message}");
        }

    }
}