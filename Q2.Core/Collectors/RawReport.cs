using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Collectors
{
    public class RawReport
    {
        public string Path { get; set; }

        public QaStructure Structure { get; set; }

        public List<CalculatedField> Fields { get; set; }
    }
}