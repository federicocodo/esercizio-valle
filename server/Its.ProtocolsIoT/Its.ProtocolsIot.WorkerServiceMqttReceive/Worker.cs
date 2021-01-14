using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Its.ProtocolsIot.WorkerServiceMqttReceive
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string topic = "/scooter/device-7/speed";
            List<string> topics = new List<string>()
                {
                    "speed",
                    "latitude",
                    "longitude"
                };

            try
            {
                var client = new MqttClient("127.0.0.1");
                client.Connect(Guid.NewGuid().ToString());
                client.Subscribe(new string[] { topic }, new byte[] { 2 });

                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine($"Topic: { e.Topic }, Message: { e.Message.ToString() }");
        }
    }
}
