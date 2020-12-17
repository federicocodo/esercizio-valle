using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Its.ProtocolsIoT.Data.Models
{
    public class ScooterTable : TableEntity
    {
        public double Speed { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ScooterTable() { }

        public ScooterTable(double speed, double latitude, double longitude)
        {
            Speed = speed;
            Latitude = latitude;
            Longitude = longitude;

            PartitionKey = "Its";
            RowKey = Guid.NewGuid().ToString();
        }

        //id dispositivo partition key
        //guid row key
    }
}
