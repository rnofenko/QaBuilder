using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public class TypedValue
    {
        public DType Type { get; set; }

        public object Value { get; set; }

        public TypedValue(string value)
        {
            Value = value;
            Type = DType.String;
        }

        public TypedValue(double value)
        {
            Value = value;
            Type = DType.Double;
        }

        public TypedValue(int value)
        {
            Value = value;
            Type = DType.Int;
        }

        public TypedValue(double value, DType type)
        {
            Value = value;
            Type = type;
        }

        public double Double()
        {
            return (double)Value;
        }
    }
}