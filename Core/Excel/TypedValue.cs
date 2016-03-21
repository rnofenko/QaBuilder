using System;
using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public class TypedValue
    {
        public DType Type { get; set; }

        public NumberFormat Format { get; set; }

        public object Value { get; set; }

        public TypedValue(string value)
        {
            Value = value;
            Type = DType.String;
        }

        public TypedValue(double value)
        {
            Value = value;
            Type = DType.Number;
            Format = NumberFormat.Double;
        }

        public TypedValue(int value)
        {
            Value = value;
            Type = DType.Number;
            Format = NumberFormat.Integer;
        }

        public TypedValue(double? value, NumberFormat formatType)
        {
            Value = value;
            Type = DType.Number;
            Format = formatType;
        }

        public static implicit operator TypedValue(string value)
        {
            return new TypedValue(value);
        }

        public static implicit operator TypedValue(int value)
        {
            return new TypedValue(value);
        }

        public static implicit operator TypedValue(double value)
        {
            return new TypedValue(value);
        }

        public double Double()
        {
            if (Value == null)
            {
                return 0;
            }
            return (double)Value;
        }

        public double? NullableDouble()
        {
            return (double?)Value;
        }

        public string String()
        {
            return (string)Value;
        }

        public long Int()
        {
            try
            {
                return (int) Value;
            }
            catch
            {
                return Convert.ToInt64(Value);
            }
        }

        public override string ToString()
        {
            return $"{Type} - {Value}";
        }
    }
}