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



/*
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    //private readonly string url = "http://localhost:52217/api";
    //private readonly string endpoint = "/scooters"; ENDPOINT HTTP
    private readonly string endpoint = "/scooter"; //ENDPOINT MQTT
    private string deviceId = "device-";

    private int numberDevices = 10;


    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            List<string> devices = new List<string>();

            for (int i = 1; i < numberDevices + 1; i++)
            {
                devices.Add(deviceId + i.ToString());
            }

            List<string> topics = new List<string>()
                {
                    "speed",
                    "latitude",
                    "longitude"
                };

            while (!stoppingToken.IsCancellationRequested)
            {

                foreach (var device in devices)
                {
                    IProtocol protocol = new MqttProtocol(device);

                    var sensor = new Sensor();
                    var scooter = sensor.GetScooter();

                    // define protocol
                    //IProtocol protocol = new HttpProtocol(url+endpoint+deviceId);

                    var jsonString = JsonSerializer.Serialize(scooter);
                    //protocol.Send(jsonString);

                    foreach (var topic in topics)
                    {
                        protocol.Subscribe(endpoint + "/" + device + "/" + topic);

                        if (topic == "speed")
                        {
                            protocol.Publish(topic, scooter.Speed.ToString());
                        }

                        if (topic == "latitude")
                        {
                            protocol.Publish(topic, scooter.Latitude.ToString());
                        }

                        if (topic == "longitude")
                        {
                            protocol.Publish(topic, scooter.Longitude.ToString());
                        }


                    }

                    Console.WriteLine("Payload: " + jsonString);
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
}*/