using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Threading.Channels;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Its.Protocols.Data.Models;
using System.Text;
using Its.Protocols.Data;

namespace Iot.Protocols.ReceiveWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _uri;
        private readonly string queueName = "hello";


        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _uri = configuration["Uri"];
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TableRepository _tableRepository = new TableRepository(_configuration);

            var factory = new ConnectionFactory() { Uri = new Uri(_uri) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var scooter = JsonSerializer.Deserialize<Scooter>(body);
                    await _tableRepository.Insert("device-1", scooter);

                    _logger.LogInformation($"Received { message }");
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
