using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class RawReportField
    {
        public FieldDescription Description { get; }

        public DType Type => Description.Type;

        public bool CalcUnique => Description.CalcUnique;

        public string Name => Description.Name;

        public double Sum { get; set; }

        public Dictionary<string, int> UniqueValues { get; set; }

        public RawReportField(FieldDescription description)
        {
            Description = description;
            UniqueValues = new Dictionary<string, int>();
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