using System.Globalization;

namespace Common.Extensions
{
    public static class NumberExtension
    {
        public static string GetDayName(this int dayNumber)
        {
            if (dayNumber < 1 || dayNumber > 7)
            {
                return "Day number must be between 1 and 7.";
            }

            // Create a reference DateTime object for Sunday
            DateTime referenceDate = new(2024, 6, 2); // June 2, 2024 is a Sunday

            // first day is sunday: dayNUmber = 0
            DateTime targetDate = referenceDate.AddDays(dayNumber);

            // Get the day name in the specified culture
            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(targetDate.DayOfWeek);
        }
    }
}