using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class RawReportField
    {
        public FieldDescription Description { get; }

        public DType Type => Description.Type;

        public bool SelectUniqueValues => Description.SelectUniqueValues;
        public Dictionary<string, int> SelectedUniqueValues { get; set; }

        public bool CountUniqueValues => Description.CountUniqueValues;
        public List<string> CountedUniqueValues { get; set; }

        public string Name => Description.Name;

        public double Sum { get; set; }
        
        public RawReportField(FieldDescription description)
        {
            Description = description;
            SelectedUniqueValues = new Dictionary<string, int>();
            CountedUniqueValues = new List<string>();
        }

        public RawReportField(string name, string title, DType type)
            :this(new FieldDescription { Name = name, Type = type, Title = title })
        {
        }

        public RawReportField Clone()
        {
            var field = new RawReportField(Description) {Sum = Sum};
            return field;
        }
    }
}