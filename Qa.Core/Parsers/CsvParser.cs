using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Qa.Core.Parsers
{
    public class CsvParser
    {
        public const string DEFAULT_DELIMITER = ",";

        private readonly Regex _regex;
        private readonly char _delimiter;
        private readonly char _textQualifier;
        private readonly char[] _buffer = new char[10000];

        public CsvParser(string delimiter, string textQualifier)
        {
            delimiter = delimiter ?? DEFAULT_DELIMITER;
            _delimiter = delimiter[0];
            if (textQualifier.IsEmpty())
            {
                textQualifier = "\"";
            }

            _textQualifier = textQualifier[0];
            if (delimiter == "|")
            {
                delimiter = "\\|";
            }
            
            var pattern = "\\s*(?:" + textQualifier + "(?<v>[^" + textQualifier + "]*(" + textQualifier + textQualifier + "[^" + textQualifier + "]*)*)" + textQualifier + "\\s*|(?<v>[^" + delimiter + "]*))(?:" + delimiter + "|$)";
            _regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
        }

        public string[] ParseRegex(string line)
        {
            var matches = _regex.Matches(line);
            var result = new string[matches.Count - (line[line.Length-1]==_delimiter ? 0 : 1)];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = matches[i].Groups[2].Value;
            }

            return result;
        }
        
        public string[] Parse(string line)
        {
            var words = new List<string>();
            var index = 0;
            var wordIsOpen = false;
            foreach (var c in line)
            {
                if (c == _delimiter && !wordIsOpen)
                {
                    words.Add(new string(_buffer, 0, index));
                    index = 0;
                }
                else if (c == _textQualifier)
                {
                    wordIsOpen = !wordIsOpen;
                }
                else
                {
                    _buffer[index] = c;
                    index++;
                }
            }
            
            words.Add(new string(_buffer, 0, index));
            return words.ToArray();
        }
    }
}
