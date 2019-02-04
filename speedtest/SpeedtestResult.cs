using System.Collections.Generic;
using System.Linq;

namespace Speedtest
{
    public class SpeedtestResult
    {
        public SpeedResult Download { get; }
        public SpeedResult Upload { get; }
        public SpeedResult Ping { get; }
        public string Source { get; }

        public SpeedtestResult(SpeedResult download, SpeedResult upload, SpeedResult ping, string source)
        {
            Download = download;
            Upload = upload;
            Ping = ping;
            Source = source;
        }
    }

    public class SpeedResult
    {
        public IList<long> Samples { get; } = new List<long>();
        public long Average => (long)Samples.Average();
    }
}