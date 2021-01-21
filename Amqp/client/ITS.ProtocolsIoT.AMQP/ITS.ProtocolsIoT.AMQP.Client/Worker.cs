using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text.Json;
using ITS.ProtocolsIoT.AMQP.Data.Models;

namespace ITS.ProtocolsIoT.AMQP.Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _uri;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _uri = configuration["Uri"];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            while (!stoppingToken.IsCancellationRequested)
            {
                Send();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
        protected void Send()
        {
            {
                var factory = new ConnectionFactory() { Uri = new Uri(_uri) };
                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {

                        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                        var rnd = new Random();
                        var scooter = new Scooter()
                        {
                            speed = rnd.Next(0, 100),
                            latitude = rnd.Next(0, 100),
                            longitude = rnd.Next(0, 100)
                        };

                        var jsonString = JsonSerializer.Serialize(scooter);

                        //string message = "Hello World!";
                        var body = Encoding.UTF8.GetBytes(jsonString);

                        channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                        Console.WriteLine(" [x] Sent {0}", jsonString);

                    }
                }
            }
        }
    }
}
