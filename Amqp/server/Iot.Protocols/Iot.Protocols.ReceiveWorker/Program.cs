using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Its.Protocols.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iot.Protocols.ReceiveWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddSingleton<ITableRepository, TableRepository>();
                    services.AddHostedService<Worker>();
                });
    }
}
