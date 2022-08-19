using System;

namespace TowerSoft.Repository.Iris {
    /// <summary>
    /// Utilities for converting to special date formats in Intersystems Iris
    /// </summary>
    public static class IrisDateUtilities {
        /// <summary>
        /// Converts a DateTime to Iris's Fileman logical date format
        /// </summary>
        /// <param name="dateTime">DateTime to convert</param>
        /// <returns>Returns the Fileman logical date</returns>
        public static string DateTimeToLogicalDate(DateTime dateTime) {
            if (dateTime.Year < 1700)
                throw new Exception("Iris is unable to store dates before the year 1700 in this format.");
            if (dateTime.Year >= 2700)
                throw new Exception("Iris is unable to store dates after the year 2699 in this format.");

            string year = dateTime.ToString("yyyy");

            int century = int.Parse(year.Substring(0, 2)) - 17;
            string dateString = dateTime.ToString("yyMMdd.HHmmss");

            // Convert to decimal then back to a string to leave off any trailing 0s after the period
            decimal decimalValue = Convert.ToDecimal($"{century}{dateString}");
            return decimalValue.ToString("0.######");
        }

        /// <summary>
        /// Converts a DateTime to MUMPS $HOROLOG date format. This is the number of days since 12/31/1840.
        /// </summary>
        /// <param name="datetime">DateTime to convert</param>
        /// <returns>Returns the $HOROLOG date</returns>
        public static int DateTimeToHorologDate(DateTime datetime) {
            if (datetime.Year < 1840)
                throw new Exception("Iris is unable to store date before the year 1841 in this format.");
            TimeSpan timespan = datetime.Subtract(new DateTime(1840, 12, 31));
            return (int)Math.Floor(timespan.TotalDays);
        }
    }
}
