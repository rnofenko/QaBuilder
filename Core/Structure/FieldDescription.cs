using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class FieldDescription
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public DType Type { get; set; }

        public NumberFormat NumberFormat { get; set; }

        public string DateFormat { get; set; }

        public SortType Sort { get; set; }

        public CalculationDescription Calculation { get; set; }

        public Dictionary<string, string> Translate { get; set; }

        public List<BinRange> Bins { get; set; }

        public override string ToString()
        {
            return $"{Name} {Type}";
        }
    }
}