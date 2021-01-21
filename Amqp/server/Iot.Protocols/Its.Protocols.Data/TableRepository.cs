using Its.Protocols.Data.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Its.Protocols.Data
{
    public class TableRepository : ITableRepository
    {
        private readonly IConfiguration _configuration;

        public TableRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Insert(string deviceId, Scooter scooter)
        {
            var cs = _configuration.GetConnectionString("Its_Storage");
            var storageAccount = CloudStorageAccount.Parse(cs);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var usersClient = tableClient.GetTableReference("scooter");
            // creazione tabella users
            //
            await usersClient.CreateIfNotExistsAsync();

            ScooterTable scooterTable = new ScooterTable(deviceId, scooter.Speed, scooter.Latitude, scooter.Longitude);
            var insertUser = TableOperation.Insert(scooterTable);
            var resultInsert = await usersClient.ExecuteAsync(insertUser);

        }
    }
}
