using System.Globalization;

namespace Qa.Core
{
    public static class DoubleExtension
    {
        public static string ToStr(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static bool BiggerOrEqualThan(this double value1, string value2)
        {
            return new StringsAsNumbersComparer().Compare(value1, value2) > -1;
        }

        public static bool LessOrEqualThan(this double value1, string value2)
        {
            return new StringsAsNumbersComparer().Compare(value1, value2) < 1;
        }
    }
}