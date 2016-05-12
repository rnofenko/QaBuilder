using System;

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
    }
}
