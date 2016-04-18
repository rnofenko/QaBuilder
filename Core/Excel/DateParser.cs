using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Qa.Core.Excel
{
    public class DateParser
    {
        public string ExtractDate(string fileName)
        {
            var rgx = new Regex(@"\d{6,}");
            var match = rgx.Match(fileName).ToString();

            var parsedDate = DateTime.Parse(match.Length < 8
                ? string.Format("{0}/01/{1}", match.Substring(4, 2), match.Substring(0, 4))
                : string.Format("{0}/{1}/{2}", match.Substring(4, 2), match.Substring(6, 2), match.Substring(0, 4)));

            var monthName = toMonthName(parsedDate.Month);

            return string.Format("{0} {1}", monthName, parsedDate.Year);
        }

        private string toMonthName(int monthNumber)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
        }
    }
}