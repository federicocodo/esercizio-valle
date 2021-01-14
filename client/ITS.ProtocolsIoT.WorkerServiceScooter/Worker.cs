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

                while (!stoppingToken.IsCancellationRequested)
                {

                    List<Scooter> sensors = new List<Scooter>();

                    var sensor = new Sensor();

                    var scooter = new Scooter();
                    scooter = sensor.GetScooter();
                    sensors.Add(scooter);


                    var random = new Random();
                    var id = random.Next(10);

                    deviceId += id.ToString();

                    // define protocol
                    IProtocol protocol = new HttpProtocol(url+endpoint+deviceId);


                    foreach (var sensorData in sensors)
                    {
                        var jsonString = JsonSerializer.Serialize(sensorData);
                        protocol.Send(jsonString);
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