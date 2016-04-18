using System.Collections.Generic;
using System.Linq;
using Qa.Core.Collectors;

namespace Qa.Core.Structure
{
    public class RawReportField
    {
        public FieldDescription Description { get; private set; }

        public DType Type { get { return Description.Type; } }

        public string Name {get { return Description.Name; } }

        public double Number { get; private set; }

        public Dictionary<string, double> GroupedNumbers { get; set; }

        public RawReportField(ParseField field)
        {
            Description = field.Description;

            if (field.UniqueValues.Any())
            {
                Number = field.UniqueValues.Count;
            }
            else
            {
                Number = field.Number;
            }

            if (field.GroupedUniqueValues.Any())
            {
                GroupedNumbers = field.GroupedUniqueValues.ToDictionary(x => x.Key, x => (double)x.Value.Count);
            }
            else
            {
                GroupedNumbers = field.GroupedNumbers;
            }
        }

        public RawReportField(FieldDescription description, double sum)
        {
            GroupedNumbers = new Dictionary<string, double>();
            Description = description;
            Number = sum;
        }

        public RawReportField Clone()
        {
            var field = new RawReportField(Description, Number);
            return field;
        }
    }
}