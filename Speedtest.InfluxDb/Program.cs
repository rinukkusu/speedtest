using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Net.Models;
using Newtonsoft.Json;
using Speedtest.Shared;

namespace Speedtest.InfluxDb
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (!Console.IsInputRedirected)
            {
                Console.WriteLine("Error: Pipe the output of Speedtest into this program.");
                return;
            }

            var input = Console.In.ReadToEnd();
        
            var influxDbUrl = Environment.GetEnvironmentVariable("INFLUXDB_URL");
            var influxDbUsername = Environment.GetEnvironmentVariable("INFLUXDB_USERNAME");
            var influxDbPassword = Environment.GetEnvironmentVariable("INFLUXDB_PASSWORD");

            if (string.IsNullOrWhiteSpace(influxDbUsername)) influxDbUsername = "root";
            if (string.IsNullOrWhiteSpace(influxDbPassword)) influxDbPassword = "root";

            var db = new InfluxDB.Net.InfluxDb(influxDbUrl, influxDbUsername, influxDbPassword);

            var result = JsonConvert.DeserializeObject<SpeedtestResult>(input);

            await db.CreateDatabaseAsync("Speedtest");
            
            var point = new Point()
            {
                Measurement = "TestResults",
                Fields = new Dictionary<string, object>()
                {
                    { "source", result?.Source ?? "n/a" },
                    { "download", result?.Download?.Average ?? 0 },
                    { "upload", result?.Upload?.Average ?? 0 },
                    { "ping", result?.Ping?.Average ?? 0 }
                },
                Timestamp = DateTime.UtcNow
            };

            var response = await db.WriteAsync("Speedtest", point);
        }
    }
}