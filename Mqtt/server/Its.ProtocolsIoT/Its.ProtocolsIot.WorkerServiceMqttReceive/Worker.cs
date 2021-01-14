using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Its.ProtocolsIoT.Data;
using Its.ProtocolsIoT.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Timer = System.Timers.Timer;

namespace Its.ProtocolsIot.WorkerServiceMqttReceive
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static MqttClient client;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string topic = "scooter/device-1/summary";

            string BrokerAddress = "127.0.0.1";

            client = new MqttClient(BrokerAddress);

            client.MqttMsgPublishReceived += PublishReceivedAsync;

            string clientId = Guid.NewGuid().ToString();

            client.Connect(clientId);

            Subscribe(topic);

            //Invia il comando di accensione monopattino
            Publish("scooter/device-1/cmd/scooter", "Scooter ON");

            //Invia il comando di apertura nuova corsa
            Publish("scooter/device-1/cmd/race", "Start race");

            //Invia il comando di accensione led di pos monopattino
            Publish("scooter/device-1/cmd/led", "Scooter LED ON");

            //Invia il comando di accensione display monopattino
            Publish("scooter/device-1/cmd/display", "Scooter Display ON");
        }

        public async void PublishReceivedAsync(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            if (ReceivedMessage != null)
            {
                var detection = JsonSerializer.Deserialize<Scooter>(ReceivedMessage);

                TableRepository tableRepository = new TableRepository(_configuration);

                await tableRepository.Insert("device-1", detection);

                _logger.LogInformation($"Message: {ReceivedMessage}");
            }

        }

        public void Subscribe(string topic)
        {
            if (topic != "")
            {

                client.Subscribe(new string[] { topic }, new byte[] { 2 });
                _logger.LogInformation($"Ok subscribed topic: {topic}");
            }
            else
            {
                _logger.LogInformation("Invalid topic.");
            }
        }

        public void Publish(string topic, string message)
        {
            if (topic != "")
            {
                client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                Console.WriteLine($"Ok published message:{message} on topic:{topic}");

            }
            else
            {
                Console.WriteLine("Invalid topic.");
            }
        }


        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Invia il comando di accensione monopattino
            Publish("scooter/device-1/cmd/scooter", "Scooter OFF");

            //Invia il comando di apertura nuova corsa
            Publish("scooter/device-1/cmd/race", "Stop race");

            //Invia il comando di accensione led di pos monopattino
            Publish("scooter/device-1/cmd/led", "Scooter LED OFF");

            //Invia il comando di accensione display monopattino
            Publish("scooter/device-1/cmd/display", "Scooter Display OFF");

        }
    }
}

