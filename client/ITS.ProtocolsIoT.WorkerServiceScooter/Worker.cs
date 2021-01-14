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
        

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                List<string> devices = new List<string>();
                List<Scooter> sensors = new List<Scooter>();
                List<string> topics = new List<string>()
                {
                    "speed",
                    "latitude",
                    "longitude"
                };

                while (!stoppingToken.IsCancellationRequested)
                {


                    var random = new Random();
                    var id = random.Next(10);

                    deviceId += id.ToString();

                    IProtocol protocol = new MqttProtocol(deviceId);

                    var sensor = new Sensor();
                    var scooter = new Scooter();
                    scooter = sensor.GetScooter();
                    sensors.Add(scooter);


                    // define protocol
                    //IProtocol protocol = new HttpProtocol(url+endpoint+deviceId);


                    foreach (var sensorData in sensors)
                    {
                        var jsonString = JsonSerializer.Serialize(sensorData);
                        //protocol.Send(jsonString);


                        //ciao
                        foreach (var topic in topics)
                        {
                            protocol.Subscribe("/scooter/device-7/" + topic);

                            if(topic == "speed")
                            {
                                protocol.Publish(topic, sensorData.Speed.ToString());
                            }
                            if (topic == "latitude")
                            {
                                protocol.Publish(topic, sensorData.Latitude.ToString());
                            }
                            if (topic == "longitude")
                            {
                                protocol.Publish(topic, sensorData.Longitude.ToString());
                            }
                            
                            
                        }
                        Console.WriteLine(jsonString);

                    }

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);

                    deviceId = "/device-";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}