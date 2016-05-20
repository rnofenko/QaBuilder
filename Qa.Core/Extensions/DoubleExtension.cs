using System.Globalization;

namespace Qa.Core
{
    public static class DoubleExtension
    {
        public static string ToStr(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}