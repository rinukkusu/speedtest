namespace Speedtest
{
    public static class LongExtensions
    {
        public static double ToMbps(this long bps)
        {
            return (double)bps / 1024 / 1024;
        }
    }
}