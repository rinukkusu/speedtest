using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Speedtest.Settings;

namespace Speedtest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection()
                .AddScoped(x =>
                {
                    var temp = new SpeedtestSettings();
                    configuration.GetSection("SpeedtestSettings").Bind(temp);

                    temp.IsOutputRedirected = Console.IsOutputRedirected;
                    return temp;
                })
                .AddScoped<SpeedtestRunner>()
                .BuildServiceProvider();

            
            var runner = services.GetService<SpeedtestRunner>();
            var settings = services.GetService<SpeedtestSettings>();

            var source = args.Length == 1 
                ? args[0] 
                : settings.DefaultTestfileSource;
            
            var result = await runner.Run(source);

            if (settings.IsOutputRedirected)
            {
                var resultJson = JsonConvert.SerializeObject(result, Formatting.Indented);
                Console.WriteLine(resultJson);
            }
            else
            {
                var download = result.Download.Average.ToMbps();
                Console.WriteLine($"Average: {download:F2} Mbps");
            }
        }
    }
}