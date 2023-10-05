namespace PriceAggregator.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static int ToEpochTime(this DateTime time)
        {
            var span = time - new DateTime(1970, 1, 1);
            return (int)span.TotalSeconds;
        }

        public static double ToEpochTimeMilliseconds(this DateTime time)
        {
            var span = time - new DateTime(1970, 1, 1);
            return span.TotalMilliseconds;
        }

        public static DateTime TrimMinutesAndLess(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }

        public static TimeSpan TillEndOfCurrentHour(this DateTime time)
        {
            var now = DateTime.UtcNow;
            var nextHour = now.AddHours(1).AddMinutes(-now.Minute).AddSeconds(-now.Second);
            var span = nextHour - now;
            return span;
        }
    }
}