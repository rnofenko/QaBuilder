using Qa.Structure;

namespace Qa.Collectors
{
    public class StatisticsField
    {
        public DType Type { get; set; }

        public StatisticsField(FieldDescription field)
        {
            Type = field.Type;
            Name = field.Name;
        }

        public string Name { get; set; }

        public double Sum { get; set; }
    }
}