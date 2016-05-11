using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Collectors.CalcFields
{
    public class CalcGroupCountField : ICalculationField
    {
        private readonly Dictionary<string, double> _groupedNumbers;
        private readonly int _index;

        public CalcGroupCountField(QaField qaField)
        {
            Field = qaField;
            _index = qaField.FieldIndex;
            _groupedNumbers = new Dictionary<string, double>();
        }

        public void Calc(string[] parts)
        {
            var value = parts[_index];

            try
            {
                _groupedNumbers[value]++;
            }
            catch
            {
                _groupedNumbers.Add(value, 1);
            }
        }

        public QaField Field { get; }

        public double GetSingleResult()
        {
            return 0;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return _groupedNumbers;
        }
    }
}