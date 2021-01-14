using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ITS.ProtocolsIoT.Data.Sensors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ITS.ProtocolsIoT.Data;
using System.Text.Json;
using ITS.ProtocolsIoT.Data.Models;
using ITS.ProtocolsIoT.Data.Protocols;

namespace ITS.ProtocolsIoT.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string url = "http://localhost:52217/api";
        private readonly string endpoint = "/scooters";
        private string deviceId = "/device-";

        private static string clientId = "device-1";
        private string topicSummary = $"scooter/{ clientId }/summary";
        private string topicRace = $"scooter/{ clientId }/cmd/race";
        private string topicScooter = $"scooter/{ clientId }/cmd/scooter";
        private string topicLed = $"scooter/{ clientId }/cmd/led";
        private string topicDisplay = $"scooter/{ clientId }/cmd/display";




        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                /*List<string> devices = new List<string>();
                List<Scooter> sensors = new List<Scooter>();
                List<string> topics = new List<string>()
                {
                    "speed",
                    "latitude",
                    "longitude"
                };*/

                Scooter scooter = new Scooter();

                IProtocol protocol = new MqttProtocol(deviceId);
                protocol.Subscribe(topicSummary);
                protocol.Subscribe(topicRace);
                protocol.Subscribe(topicScooter);
                protocol.Subscribe(topicLed);
                protocol.Subscribe(topicDisplay);




                while (!stoppingToken.IsCancellationRequested)
                {
                    if (protocol.ScooterOn)
                    {
                        if (protocol.Race)
                        {
                            var sensor = new Sensor();
                            scooter = sensor.GetScooter();

                            var jsonString = JsonSerializer.Serialize(scooter);

                            protocol.Publish(topicSummary, jsonString);
                            _logger.LogInformation($"{ topicSummary }, { jsonString }");

                        }
                        else
                        {
                            var sensor = new Sensor();
                            scooter = sensor.GetScooter();
                            scooter.Speed = 0;

                            var jsonString = JsonSerializer.Serialize(scooter);

                            protocol.Publish(topicSummary, jsonString);
                        }

                    }

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}