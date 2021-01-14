using ITS.ProtocolsIoT.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITS.ProtocolsIoT.Data.Sensors
{
    public class Sensor : ISensor
    {

        public Scooter GetScooter()
        {
            var random = new Random();

            var scooter = new Scooter()
            {
                Speed = random.Next(100),
                Latitude = random.Next(100),
                Longitude = random.Next(100)
            };

            return scooter;
            
        }
    }
}
