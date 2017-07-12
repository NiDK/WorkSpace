using System;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Client.Helper
{
    public static class DateTimeUtility
    {
        public static readonly DateTime TimestampBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime Now
        {
            get { return DateTime.UtcNow; }
        }

        public static ulong CurrentTimestamp
        {
            get { return ToTimestamp(Now); }
        }

        public static ulong ToTimestamp(DateTime dateTime)
        {
            return (ulong)(dateTime.ToUniversalTime() - TimestampBase).TotalSeconds;
        }

        public static DateTime FromTimestamp(string timestamp)
        {
            ArgumentHelper.AssertNotEmpty(timestamp);

            ulong seconds;
            if (ulong.TryParse(timestamp, out seconds))
                return FromTimestamp(seconds);

            throw new ArgumentOutOfRangeException("timestamp", "Invalid timestamp: " + timestamp);
        }

        public static DateTime FromTimestamp(ulong timestamp)
        {
            return TimestampBase.AddSeconds(timestamp);
        }
    }
}
