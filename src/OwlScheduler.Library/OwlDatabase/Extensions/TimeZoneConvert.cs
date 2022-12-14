using System;

namespace OwlScheduler.Library.OwlDatabase.Extensions
{
    public static class TimeZoneConvert
    {
        public static DateTime MySqlDateTimeFromUtc(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcTime, DateTimeKind.Utc), TimeZoneInfo.Local); 
        }
    }
}