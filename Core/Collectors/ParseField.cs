using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Collectors
{
    public class ParseField
    {
        public ParseField(FieldDescription field)
        {
            Description = field;
            CountedUniqueValues = new HashSet<string>();
            GroupedNumbers = new Dictionary<string, double>();
        }

        public FieldDescription Description { get; set; }

        public DType Type => Description.Type;

        public double Number { get; set; }

        public Dictionary<string, double> GroupedNumbers { get; set; }

        public CalculationDescription Calculation => Description.Calculation;

        public HashSet<string> CountedUniqueValues { get; set; }
    }
}