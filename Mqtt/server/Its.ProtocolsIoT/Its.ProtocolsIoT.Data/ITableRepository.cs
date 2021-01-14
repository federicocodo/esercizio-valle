using Its.ProtocolsIoT.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Its.ProtocolsIoT.Data
{
    public interface ITableRepository
    {
        Task Insert(string deviceId, Scooter scooter);
    }
}
