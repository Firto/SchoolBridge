using System;

public static class DateTimeExtensions
{
    public static long ToUnixTimestamp(this DateTime date)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var time = date.ToUniversalTime().Subtract(epoch);
        return time.Ticks / TimeSpan.TicksPerSecond;
    }
}