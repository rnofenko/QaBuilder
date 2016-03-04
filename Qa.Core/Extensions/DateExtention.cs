using System.Globalization;

namespace Qa.Core
{
    public static class DateExtention
    {
        public static string ToMonthName(int monthNumber)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
        }

        public static string ToShortMonthName(int monthNumber)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthNumber);
        }
    }
}
