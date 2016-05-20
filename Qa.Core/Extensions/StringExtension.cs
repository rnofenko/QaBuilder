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

        public static bool LessOrEqualThan(this string value1, string value2)
        {
            return new StringsAsNumbersComparer().Compare(value1, value2) < 1;
        }
    }
}
