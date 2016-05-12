using Qa.Core;
using Qa.Core.Structure;

namespace Q2.Core.Structure.Json
{
    public class JsonField
    {
        public string Name { get; set; }

        public DType Type { get; set; }

        public NumberFormat NumberFormat { get; set; }

        public string DateFormat { get; set; }

        public Field Convert()
        {
            var field = new Field {Name = Name, Type = Type, NumberFormat = NumberFormat, DateFormat = DateFormat};

            if (field.Type == DType.None)
            {
                if (field.DateFormat.IsNotEmpty())
                {
                    field.Type = DType.Date;
                }
                else if (field.NumberFormat == NumberFormat.None)
                {
                    field.Type = DType.String;
                }
                else
                {
                    field.Type = DType.Numeric;
                }
            }

            if (field.NumberFormat == NumberFormat.None && field.Type == DType.Numeric)
            {
                field.NumberFormat = NumberFormat.Double;
            }
            
            return field;
        }
    }
}