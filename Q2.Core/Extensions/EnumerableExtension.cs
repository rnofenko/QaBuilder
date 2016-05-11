using System.Collections.Generic;
using System.Linq;

namespace Q2.Core.Extensions
{
    public static class EnumerableExtension
    {
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }
    }
}
