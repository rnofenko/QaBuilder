namespace Qa.Core
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool BiggerThan(this string value1, string value2)
        {
            return new StringsAsNumbersComparer().Compare(value1, value2) == 1;
        }

        public static bool BiggerOrEqualThan(this string value1, string value2)
        {
            return new StringsAsNumbersComparer().Compare(value1, value2) > -1;
        }

        public static bool BiggerOrEqualThan(this string value1, double value2)
        {
            return new StringsAsNumbersComparer().Compare(value1, value2) > -1;
        }

        public static bool LessOrEqualThan(this string value1, string value2)
        {
            return new StringsAsNumbersComparer().Compare(value1, value2) < 1;
        }

        public static bool LessOrEqualThan(this string value1, double value2)
        {
            return new StringsAsNumbersComparer().Compare(value1, value2) < 1;
        }

        public static string IfEmpty(this string value1, string value2 = null, string value3 = null, string value4 = null)
        {
            if (value1.IsNotEmpty())
            {
                return value1;
            }
            if (value2.IsNotEmpty())
            {
                return value2;
            }
            if (value3.IsNotEmpty())
            {
                return value3;
            }
            return value4;
        }
    }
}
