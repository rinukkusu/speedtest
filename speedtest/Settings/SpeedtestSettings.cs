using System;
using System.Collections.Generic;
using System.Linq;

namespace Speedtest.Settings
{
    public class SpeedtestSettings
    {
        public List<Testfile> TestFiles { get; set; }
        public string DefaultTestfileSource { get; set; }
        public bool IsOutputRedirected { get; set; }

        public Testfile GetTestfileBySource(string source)
        {
            var runSettings = TestFiles.FirstOrDefault(x => x.Source == source);
            return runSettings ?? throw new Exception($"The test source {source} doesn't exist.");
        }
    }

    public class Testfile
    {
        public string Source { get; set; }
        public string Url { get; set; }
    }
}