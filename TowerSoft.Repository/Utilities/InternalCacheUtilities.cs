using System;

namespace TowerSoft.Repository.Utilities {
    internal static class InternalCacheUtilities {
        internal static string DateTimeToLogicalDate(DateTime dateTime) {
            if (dateTime.Year < 1700)
                throw new Exception("Cache is unable to store dates before the year 1700 in this format.");
            if (dateTime.Year >= 2700)
                throw new Exception("Cache is unable to store dates after the year 2699 in this format.");

            string year = dateTime.ToString("yyyy");

            int century = int.Parse(year.Substring(0, 2)) - 17;
            string dateString = dateTime.ToString("yyMMdd.HHmmss");

            return $"{century}{dateString}";
        }

        internal static int DateTimeToHorologDate(DateTime datetime) {
            if (datetime.Year < 1840)
                throw new Exception("Cache is unable to store date before the year 1841 in this format.");
            TimeSpan timespan = datetime.Subtract(new DateTime(1840, 12, 31));
            return (int)Math.Floor(timespan.TotalDays);
        }
    }
}
