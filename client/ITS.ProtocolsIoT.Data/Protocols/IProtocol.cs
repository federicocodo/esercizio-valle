using System;
using System.Collections.Generic;
using System.Text;

namespace ITS.ProtocolsIoT.Data
{
    public interface IProtocol
    {
        void Send(string data);

        void Subscribe(string topic);

        void Publish(string topic, string message);
    }
}
