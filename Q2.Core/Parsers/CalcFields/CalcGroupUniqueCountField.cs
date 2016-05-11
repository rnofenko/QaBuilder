using System.Collections.Generic;
using System.Linq;
using Q2.Core.Structure;

namespace Q2.Core.Collectors.CalcFields
{
    public class CalcGroupUniqueCountField : CalcBaseField, ICalculationField
    {
        private readonly Dictionary<string, HashSet<string>> _groupedUniqueValues;
        private readonly int _index;

        public CalcGroupUniqueCountField(QaField field) : base(field)
        {
            _groupedUniqueValues = new Dictionary<string, HashSet<string>>();
            _index = field.FieldIndex;
        }

        public void Calc(string[] parts)
        {
            var value = parts[_index];

            HashSet<string> set;
            if (!_groupedUniqueValues.TryGetValue(value, out set))
            {
                set = new HashSet<string>();
                _groupedUniqueValues.Add(value, set);
            }

            var fieldValue = parts[_index];
            if (!set.Contains(fieldValue))
            {
                set.Add(fieldValue);
            }
        }

        public double GetSingleResult()
        {
            return 0;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return _groupedUniqueValues.ToDictionary(x => x.Key, x => (double)x.Value.Count);
        }
    }
}