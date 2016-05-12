using System.Collections.Generic;
using Q2.Core.Structure;
using Qa.Core.Structure;

namespace Qa.Core.Parsers
{
    public class ParsedFile
    {
        public string SplitValue { get; set; }

        public string Path { get; set; }

        public QaStructure Structure { get; set; }

        public List<CalculatedField> Fields { get; set; }
    }
}