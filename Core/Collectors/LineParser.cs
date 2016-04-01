using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Qa.Core.Collectors
{
    public class LineParser
    {
        private string _regexPattern;

        public LineParser(string delimiter)
        {
            _regexPattern = "((?<=\")[^\"]*(?=\"(" + delimiter + "|$)+)|(?<=" + delimiter + "|^)[^" + delimiter + "\"]*(?=" + delimiter + "|$))";
        }

        public string[] Parse(string line)
        {
            /*var match = Regex.Match(line, _regexPattern);
            while(match.)*/
            return null;
        }
    }
}
