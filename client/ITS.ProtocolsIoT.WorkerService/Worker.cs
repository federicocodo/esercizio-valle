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

                    // define protocol
                    IProtocol protocol = new HttpProtocol("http://localhost:50441/api/scooters");


                    foreach (var sensorData in sensors)
                    {
                        var jsonString = JsonSerializer.Serialize(sensorData);
                        protocol.Send(jsonString);
                        Console.WriteLine(jsonString);
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
