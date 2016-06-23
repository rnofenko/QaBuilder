using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Qa.Core.Parsers
{
    public class FileDateExtractor
    {
        public DateTime? ExtractDate(string fileName)
        {
            var match = new Regex(@"\d{8}").Match(fileName);
            if (match.Success)
            {
                return parseFullDate(match.Value, "yyyyMMdd");
            }

            return null;
        }

        public DateTime? ExtractMonth(string fileName)
        {
            var match = new Regex(@"\d{6}").Match(fileName);
            if (match.Success)
            {
                return parseMonth(match.Value, "yyyyMM");
            }

            match = new Regex(@"\d{4}-\d{2}").Match(fileName);
            if (match.Success)
            {
                return parseMonth(match.Value, "yyyy-MM");
            }

            return null;
        }

        private DateTime parseFullDate(string source, string parsePattern)
        {
            return DateTime.ParseExact(source, parsePattern, CultureInfo.InvariantCulture);
        }

        private DateTime parseMonth(string source, string parsePattern)
        {
            return DateTime.ParseExact(source, parsePattern, CultureInfo.InvariantCulture);
        }
    }
}