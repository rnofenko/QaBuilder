using System;
using System.Text.RegularExpressions;

namespace Qa.Core.Parsers
{
    public static class NumberParser
    {
        public static double Parse(string value)
        {
            if (value.Length == 0)
            {
                return 0;
            }

            try
            {
                return double.Parse(value);
            }
            catch
            {
                throw new InvalidOperationException(string.Format("[{0}] is not a numeric format.", value));
            }
        }

        public static double SafeParse(string value)
        {
            if (value.Length == 0)
            {
                return 0;
            }

            try
            {
                return double.Parse(value);
            }
            catch
            {
                return 0;
            }
        }
        
        public static double? ExtractNumber(string str)
        {
            if (str == null)
            {
                return null;
            }

            try
            {
                return Parse(str);
            }
            catch
            {
                var rgx = new Regex(@"^\d{1,}");
                var match = rgx.Match(str).ToString();
                int res;
                if (int.TryParse(match, out res))
                {
                    return res;
                }
                return null;
            }
        }
    }
}
