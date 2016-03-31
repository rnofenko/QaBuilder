using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Qa.Core
{
    public class StringsAsNumbersComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            var value1 = parse(s1);
            var value2 = parse(s2);
            if (value1 == null && value2 == null)
            {
                return string.Compare(s1, s2, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
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

        private int? parse(string str)
        {
            var rgx = new Regex(@"^\d{1,2}");
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
