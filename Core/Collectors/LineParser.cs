using System.Text.RegularExpressions;

namespace Qa.Core.Collectors
{
    public class LineParser
    {
        private readonly Regex _regex;
        private readonly string _delimiter;

        public LineParser(string delimiter, string textQualifier)
        {
            _delimiter = delimiter;
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
            var result = new string[matches.Count - (line.EndsWith(_delimiter) ? 0 : 1)];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = matches[i].Groups[2].Value;
            }

            return result;
        }
    }
}
