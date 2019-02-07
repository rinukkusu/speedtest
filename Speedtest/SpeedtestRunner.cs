using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Speedtest.Settings;
using Speedtest.Shared;

namespace Speedtest
{
    public class SpeedtestRunner
    {
        private readonly SpeedtestSettings _settings;
        private readonly HttpClient _client;

        private const int BufferSize = 8192 * 8;

        public SpeedtestRunner(SpeedtestSettings settings)
        {
            _settings = settings;
            _client = new HttpClient(
                new HttpClientHandler
                {
                    MaxConnectionsPerServer = 8
                });
        }

        public async Task<SpeedtestResult> Run(string source)
        {
            var runSettings = _settings.GetTestfileBySource(source);

            SpeedResult speedResult = null;
            SpeedResult pingResult = null;
            
            try {
                speedResult = await TestDownloadSpeed(runSettings.Url);
            }
            catch(Exception){}
            try {
                pingResult = await TestPing(runSettings.Url);
            }
            catch(Exception){}

            return new SpeedtestResult(speedResult, null, pingResult, source);
        }


        private async Task<SpeedResult> TestDownloadSpeed(string url)
        {
            var sw = new Stopwatch();

            using (var response = _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                response.EnsureSuccessStatusCode();
                var speedResult = new SpeedResult();

                using (var contentStream = await response.Content.ReadAsStreamAsync())
                {
                    sw.Start();

                    var buffer = new byte[BufferSize];
                    var isMoreToRead = true;
                    var tempBytesRead = 0L;
                    var tempWatch = new Stopwatch();
                    tempWatch.Start();

                    do
                    {
                        var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            tempBytesRead += read;

                            if (tempWatch.Elapsed.TotalSeconds >= 1)
                            {
                                var bps = (long) (tempBytesRead / tempWatch.Elapsed.TotalSeconds * 8);
                                speedResult.Samples.Add(bps);
                                tempWatch.Restart();
                                tempBytesRead = 0;

                                if (!_settings.IsOutputRedirected)
                                {
                                    Console.WriteLine($"{bps.ToMbps():F2} Mbps");
                                }
                            }
                        }
                    } while (isMoreToRead && sw.Elapsed <= TimeSpan.FromSeconds(15));
                }

                sw.Stop();

                return speedResult;
            }
        }

        private async Task<SpeedResult> TestPing(string url)
        {
            var parsedUrl = new Uri(url);
            using (var ping = new Ping())
            {
                var speedResult = new SpeedResult();

                for (var i = 0; i < 4; i++)
                {
                    var result = await ping.SendPingAsync(parsedUrl.Host);
                    speedResult.Samples.Add(result.RoundtripTime);
                }

                return speedResult;
            }
        }
    }
}