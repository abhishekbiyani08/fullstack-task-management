using System;
using Microsoft.Extensions.Configuration;

namespace TaskManagementApi.Helpers
{
    public class TimeZoneHelper
    {
        private readonly TimeZoneInfo _defaultTimeZone;

        public TimeZoneHelper(IConfiguration configuration)
        {
            try
            {
                _defaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }
            catch
            {
                try
                {
                    _defaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
                }
                catch
                {
                    _defaultTimeZone = TimeZoneInfo.Utc;
                }
            }
        }

        public DateTime ToLocalTime(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, _defaultTimeZone);
        }

        public DateTime ToUtcTime(DateTime localDateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, _defaultTimeZone);
        }
    }
}