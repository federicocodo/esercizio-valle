using ITS.ProtocolsIoT.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITS.ProtocolsIoT.Data.Sensors
{
    public interface ISensor
    {
        Scooter GetScooter();
    }
}
