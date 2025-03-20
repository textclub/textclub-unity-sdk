using System;
using System.Globalization;

namespace Textclub
{
    public static class DateTimeExtensions
    {
        public static string ToJsString(this DateTime dateTime)
        {
            return dateTime.ToString("o"); // Returns ISO 8601 format
        }

        public static DateTime FromJsString(string isoString)
        {
            return DateTime.Parse(isoString, null, DateTimeStyles.RoundtripKind);
        }
    }
}
