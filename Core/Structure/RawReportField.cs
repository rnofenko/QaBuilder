using System.Collections.Generic;
using Qa.Core.Collectors;

namespace Qa.Core.Structure
{
    public class RawReportField
    {
        public FieldDescription Description { get; }

        public DType Type => Description.Type;

        public bool SelectUniqueValues => Description.SelectUniqueValues;
        public Dictionary<string, double> SelectedUniqueValues { get; }

        public bool CountUniqueValues => Description.CountUniqueValues;
        public int UniqueValuesCount { get; }

        public string Name => Description.Name;

        public double Sum { get; }

        public Dictionary<string, double> GroupedSum { get; set; }

        public RawReportField(ParseField field)
        {
            Description = field.Description;
            SelectedUniqueValues = field.SelectedUniqueValues;
            UniqueValuesCount = field.CountedUniqueValues.Count;
            Sum = field.Sum;
            GroupedSum = field.GroupedSum;
        }

        public RawReportField(FieldDescription description, double sum)
        {
            Description = description;
            Sum  = sum;
        }

        public RawReportField Clone()
        {
            var field = new RawReportField(Description, Sum);
            return field;
        }
    }
}