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
            SelectedUniqueValues = new Dictionary<string, double>();
            GroupedSum = new Dictionary<string, double>();
        }

        public FieldDescription Description { get; set; }

        public DType Type => Description.Type;

        public double Sum { get; set; }
        public Dictionary<string, double> GroupedSum { get; set; }

        public bool SelectUniqueValues => Description.SelectUniqueValues;
        public Dictionary<string, double> SelectedUniqueValues { get; set; }

        public CalculationDescription Calculation => Description.Calculation;
        public HashSet<string> CountedUniqueValues { get; set; }
    }
}