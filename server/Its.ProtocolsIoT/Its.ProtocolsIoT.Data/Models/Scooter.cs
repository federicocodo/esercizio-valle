using System;
using System.Collections.Generic;
using System.Text;

namespace Its.ProtocolsIoT.Data.Models
{
    public class Scooter
    {
        public double Speed { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Scooter() { }

        public Scooter(double speed, double latitude, double longitude)
        {
            Speed = speed;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
