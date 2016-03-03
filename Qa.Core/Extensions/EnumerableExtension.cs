using System.Collections.Generic;
using System.Linq;

namespace Qa.Core
{
    public static class EnumerableExtension
    {
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
    }
}
