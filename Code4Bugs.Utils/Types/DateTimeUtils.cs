using System;

namespace Code4Bugs.Utils.Types
{
    public static class DateTimeUtils
    {
        private static readonly DateTime _unixZeroPoint = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static int ToUnixEpoch(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() - _unixZeroPoint;
            return (int)unixTime.TotalSeconds;
        }

        public static DateTime ToDateTime(this int unixEpoch)
        {
            var unixTime = _unixZeroPoint + TimeSpan.FromSeconds(unixEpoch);
            return unixTime.ToLocalTime();
        }

        public static string ToDateTimeString(this DateTime dateTime)
        {
            return $"{ToTimeString(dateTime)} - {ToDateString(dateTime)}";
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }
    }
}