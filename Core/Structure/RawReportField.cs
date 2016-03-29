using System.Collections.Generic;
using System.Linq;
using Qa.Core.Collectors;

namespace Qa.Core.Structure
{
    public class RawReportField
    {
        public FieldDescription Description { get; }

        public DType Type => Description.Type;

        public NumberFormat NumberFormat => Description.NumberFormat;
        
        public string Name => Description.Name;

        public double Number { get; }

        public Dictionary<string, double> GroupedNumbers { get; set; }

        public RawReportField(ParseField field)
        {
            Description = field.Description;
            if (field.CountedUniqueValues.Any())
            {
                Number = field.CountedUniqueValues.Count;
            }
            else
            {
                Number = field.Number;
            }            
            GroupedNumbers = field.GroupedNumbers;
        }

        public RawReportField(FieldDescription description, double sum)
        {
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