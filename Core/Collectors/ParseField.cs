using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Collectors
{
    public class ParseField
    {
        public ParseField(FieldDescription field)
        {
            Description = field;
        }

        public FieldDescription Description { get; set; }

        public DType Type => Description.Type;

        public double Sum { get; set; }

        public bool SelectUniqueValues => Description.SelectUniqueValues;
        public Dictionary<string, int> SelectedUniqueValues { get; set; }

        public bool CountUniqueValues => Description.CountUniqueValues;
        public HashSet<string> CountedUniqueValues { get; set; }
    }
}