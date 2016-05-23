using System.Collections.Generic;
using System.Globalization;
using Qa.Core.Parsers;

namespace Qa.Core
{
    public class StringsAsNumbersComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            var value1 = NumberParser.ExtractNumber(s1);
            var value2 = NumberParser.ExtractNumber(s2);
            if (value1 == null && value2 == null)
            {
                return string.Compare(s1, s2, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
            }
            if (value2 == null)//it means that value1 is number and value2 is string. number is always less then string.
            {
                return -1;
            }
            if (value1 == null)
            {
                return 1;
            }
            if (value1 > value2)
            {
                return 1;
            }
            if (value1 < value2)
            {
                return -1;
            }
            return 0;
        }

        public int Compare(string s1, double value2)
        {
            var value1 = NumberParser.ExtractNumber(s1);
            if (value1 == null)
            {
                return 1;
            }
            if (value1 > value2)
            {
                return 1;
            }
            if (value1 < value2)
            {
                return -1;
            }
            return 0;
        }

        public int Compare(double value1, string s2)
        {
            var value2 = NumberParser.ExtractNumber(s2);
            if (value2 == null)
            {
                return -1;
            }
            if (value1 > value2)
            {
                return 1;
            }
            if (value1 < value2)
            {
                return -1;
            }
            return 0;
        }
    }
}
