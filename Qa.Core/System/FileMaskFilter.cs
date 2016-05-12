using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Q2.Core.Extensions;

namespace Q2.Core.System
{
    public class FileMaskFilter
    {
        public List<string> Filter(string fileMask, string[] path)
        {
            return path.Where(x => IsMatch(fileMask, x)).ToList();
        }

        public bool IsMatch(string fileMask, string path)
        {
            if (fileMask.IsEmpty())
            {
                return true;
            }

            var mask = new Regex(
                '^' +
                fileMask
                    .Replace(".", "[.]")
                    .Replace("*", ".*")
                    .Replace("?", ".")
                + '$',
                RegexOptions.IgnoreCase);

            var fileName = Path.GetFileName(path);
            return !string.IsNullOrWhiteSpace(fileName) && mask.IsMatch(fileName);
        }
    }
}
