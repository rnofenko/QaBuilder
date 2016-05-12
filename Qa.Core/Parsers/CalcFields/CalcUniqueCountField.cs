using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Collectors.CalcFields
{
    public class CalcUniqueCountField : CalcBaseField, ICalculationField
    {
        private readonly HashSet<string> _uniqueValues;
        private readonly int _index;

        public CalcUniqueCountField(QaField field) : base(field)
        {
            _index = field.FieldIndex;
            _uniqueValues = new HashSet<string>();
        }

        public void Calc(string[] parts)
        {
            var value = parts[_index];

            if (!_uniqueValues.Contains(value))
            {
                _uniqueValues.Add(value);
            }
        }

        public double GetSingleResult()
        {
            return _uniqueValues.Count;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return null;
        }
    }
}