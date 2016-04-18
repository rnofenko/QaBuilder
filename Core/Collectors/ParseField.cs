using System;
using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Collectors
{
    public class ParseField
    {
        public ParseField(FieldDescription field)
        {
            Description = field;
            UniqueValues = new HashSet<string>();
            GroupedNumbers = new Dictionary<string, double>();
            GroupedUniqueValues = new Dictionary<string, HashSet<string>>();
        }

        public FieldDescription Description { get; set; }

        public DType Type
        {
            get { return Description.Type; }
        }

        public double Number { get; set; }

        public Dictionary<string, double> GroupedNumbers { get; set; }

        public CalculationDescription Calculation
        {
            get { return Description.Calculation; }
        }

        public HashSet<string> UniqueValues { get; set; }

        public Dictionary<string,HashSet<string>> GroupedUniqueValues { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Description.Name);
        }
    }
}