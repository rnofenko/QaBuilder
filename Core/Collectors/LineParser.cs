using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Qa.Core.Collectors
{
    public class LineParser
    {
        private readonly string _pattern;

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
            
            _pattern = "((?<="+ textQualifier + ")[^" + textQualifier + "]*(?=" + textQualifier + "(" + delimiter + "|$)+)|(?<=" + delimiter + "|^)[^" + delimiter + textQualifier +"]*(?=" + delimiter + "|$))";
        }

        public string[] Parse(string line)
        {
            var matches = Regex.Matches(line, _pattern);
            var result = new string[matches.Count];
            for(var i=0;i<matches.Count;i++)
            {
                result[i] = matches[i].Value;
            }

            return result;
        }
    }
}
