using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Its.ProtocolsIoT.Data.Models
{
    public class ScooterTable : TableEntity
    {
        public string DeviceId { get; set; }
        public double Speed { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public ScooterTable(string deviceId, double speed, double latitude, double longitude)
        {
            Speed = speed;
            Latitude = latitude;
            Longitude = longitude;
            DeviceId = deviceId;

            PartitionKey = Guid.NewGuid().ToString(); ;
            RowKey = DeviceId;
        }

        //id dispositivo partition key
        //guid row key
    }
}
