using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Qa.Core.Excel
{
    public class FileDateParser
    {
        public string ExtractDate(string fileName)
        {
            var match = new Regex(@"\d{8}").Match(fileName);
            if (match.Success)
            {
                return parseAndFormatFullDate(match.Value, "yyyyMMdd");
            }

            match = new Regex(@"\d{6}").Match(fileName);
            if (match.Success)
            {
                return parseAndFormatMonth(match.Value, "yyyyMM");
            }

            match = new Regex(@"\d{4}-\d{2}").Match(fileName);
            if (match.Success)
            {
                return parseAndFormatMonth(match.Value, "yyyy-MM");
            }

            return fileName;
        }

        private string parseAndFormatFullDate(string source, string parsePattern)
        {
            var date = DateTime.ParseExact(source, parsePattern, CultureInfo.InvariantCulture);
            return string.Format("{0:MMMM d, yyyy}", date);
        }

        private string parseAndFormatMonth(string source, string parsePattern)
        {
            var date = DateTime.ParseExact(source, parsePattern, CultureInfo.InvariantCulture);
            return string.Format("{0:MMMM yyyy}", date);
        }
    }
}