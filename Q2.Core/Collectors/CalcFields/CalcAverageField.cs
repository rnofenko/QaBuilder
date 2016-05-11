using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Collectors.CalcFields
{
    public class CalcAverageField : ICalculationField
    {
        private double _sum;
        private int _count;
        private readonly int _index;

        public CalcAverageField(QaField field)
        {
            Field = field;
            _index = field.FieldIndex;
        }

        public void Calc(string[] parts)
        {
            var value = parts[_index];
            _sum += NumberParser.Parse(value);
            _count++;
        }

        public QaField Field { get; }

        public double GetSingleResult()
        {
            return _sum / _count;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return null;
        }
    }
}