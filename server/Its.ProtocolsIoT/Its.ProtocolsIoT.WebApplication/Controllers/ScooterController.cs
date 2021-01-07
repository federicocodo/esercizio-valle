using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Its.ProtocolsIoT.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Its.ProtocolsIoT.WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScooterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ScooterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<ScooterController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ScooterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ScooterController>

        [HttpPost("{deviceId}")]
        public void Post(string deviceId, [FromBody] Scooter scooter)
        {
            var cs = _configuration.GetConnectionString("Its_Storage");
            var storageAccount = CloudStorageAccount.Parse(cs);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var messagesTable = tableClient.GetTableReference("scooter");

            messagesTable.CreateIfNotExistsAsync();

            var message = new ScooterTable(deviceId, scooter.Speed, scooter.Latitude, scooter.Longitude);
            var insertMessage = TableOperation.Insert(message);
            messagesTable.ExecuteAsync(insertMessage).ConfigureAwait(false);
        }

        // PUT api/<ScooterController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ScooterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
