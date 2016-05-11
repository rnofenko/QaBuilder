using System.Collections.Generic;
using Q2.Core.Collectors.CalcFields;

namespace Q2.Core.Structure
{
    public class CalculatedField
    {
        public QaField Field { get; private set; }

        public double Number { get; private set; }

        public Dictionary<string, double> GroupedNumbers { get; set; }

        public CalculatedField(ICalculationField field)
        {
            Field = field.Field;
            Number = field.GetSingleResult();
            GroupedNumbers = field.GetGroupedResult() ?? new Dictionary<string, double>();
        }

        public CalculatedField(QaField description, double sum)
        {
            GroupedNumbers = new Dictionary<string, double>();
            Field = description;
            Number = sum;
        }

        public CalculatedField Clone()
        {
            var field = new CalculatedField(Field, Number);
            return field;
        }
    }
}