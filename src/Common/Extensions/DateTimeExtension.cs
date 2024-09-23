using System.Globalization;

namespace Common.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToStringId(this DateTime dateTime, string format = "")
        {
            if (string.IsNullOrEmpty(format))
                format = "dd-MM-yyyy";
            return dateTime.ToString(format);
        }
        public static string ToLongStringId(this DateTime dateTime)
        {
            return dateTime.ToString("dddd, dd MMM yyyy HH:mm:ss");
        }
        public static string ToHM(this TimeSpan timeSpan)
        {
            int hours = timeSpan.Hours % 24; // Get the hour part within a day
            int minutes = timeSpan.Minutes;
            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                // Determine AM or PM
                string amPm = hours >= 12 ? "PM" : "AM";

                // Convert to 12-hour format
                if (hours == 0)
                {
                    hours = 12; // Midnight case
                }
                else if (hours > 12)
                {
                    hours -= 12; // Convert to 12-hour format
                }

                // Format the TimeSpan as a time of day with AM/PM
                return $"{hours:D2}:{minutes:D2} {amPm}";
            }
            else
            {
                return $"{hours:D2}:{minutes:D2}";
            }


        }
        public static string ToHMS(this TimeSpan timeSpan)
        {
            int hours = timeSpan.Hours % 24; // Get the hour part within a day
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                // Determine AM or PM
                string amPm = hours >= 12 ? "PM" : "AM";

                // Convert to 12-hour format
                if (hours == 0)
                {
                    hours = 12; // Midnight case
                }
                else if (hours > 12)
                {
                    hours -= 12; // Convert to 12-hour format
                }

                // Format the TimeSpan as a time of day with AM/PM
                return $"{hours:D2}:{minutes:D2}:{seconds:D2}  {amPm}";
            }
            else
            {
                return $"{hours:D2}:{minutes:D2}:{seconds:D2} ";
            }


        }
    }
}