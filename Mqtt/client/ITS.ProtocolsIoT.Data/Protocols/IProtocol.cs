using System;
using System.Collections.Generic;
using System.Text;

namespace ITS.ProtocolsIoT.Data
{
    public interface IProtocol
    {
        bool ScooterOn { get; set; }
        bool Race { get; set; }

        void Subscribe(string topic);

        void Publish(string topic, string message);
    }
}
