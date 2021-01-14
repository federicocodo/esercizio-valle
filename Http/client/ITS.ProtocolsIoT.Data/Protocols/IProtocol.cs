using System;
using System.Collections.Generic;
using System.Text;

namespace ITS.ProtocolsIoT.Data.Protocols
{
    public interface IProtocol
    {
        void Send(string data);

    }
}
