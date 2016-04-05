using System.Text.RegularExpressions;

namespace Qa.Core.Collectors
{
    public class LineParser
    {
        private readonly Regex _regex;

        public LineParser(string delimiter, string textQualifier)
        {
            if (delimiter == "|")
            {
                delimiter = "\\|";
            }

            if (textQualifier.IsEmpty())
            {
                textQualifier = "\"";
            }
            
            var pattern = "\\s*(?:" + textQualifier + "(?<v>[^" + textQualifier + "]*(" + textQualifier + textQualifier + "[^" + textQualifier + "]*)*)" + textQualifier + "\\s*|(?<v>[^" + delimiter + "]*))(?:" + delimiter + "|$)";
            _regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
        }

        public string[] Parse(string line)
        {
            var matches = _regex.Matches(line);
            var result = new string[matches.Count - 1];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = matches[i].Groups[2].Value;
            }

            return result;
        }
    }
}
