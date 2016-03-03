using Qa.Core.Structure;
using Qa.Structure;

namespace Qa.Collectors
{
    public class RawReportField
    {
        public DType Type { get; set; }

        public RawReportField(FieldDescription field)
        {
            Type = field.Type;
            Name = field.Name;
        }

        public RawReportField(RawReportField field)
        {
            Type = field.Type;
            Name = field.Name;
        }

        public string Name { get; set; }

        public double Sum { get; set; }
    }
}