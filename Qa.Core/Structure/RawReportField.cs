namespace Qa.Core.Structure
{
    public class RawReportField
    {
        public FieldDescription Description { get; }

        public DType Type => Description.Type;

        public string Name => Description.Name;

        public double Sum { get; set; }

        public RawReportField(FieldDescription description)
        {
            Description = description;
        }

        public RawReportField(string name, string title, DType type)
        {
            Description = new FieldDescription {Name = name, Type = type, Title = title};
        }

        public RawReportField Clone()
        {
            var field = new RawReportField(Description) {Sum = Sum};
            return field;
        }
    }
}