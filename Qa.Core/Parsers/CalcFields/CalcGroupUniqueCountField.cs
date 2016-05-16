using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcGroupUniqueCountField : CalcBaseGroupField, ICalculationField
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
            var key = GetKey(parts);

            HashSet<string> set;
            if (!_groupedUniqueValues.TryGetValue(key, out set))
            {
                set = new HashSet<string>();
                _groupedUniqueValues.Add(key, set);
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
            return _groupedUniqueValues.ToDictionary(x => x.Key.Replace(SEPARATOR, " / "), x => (double)x.Value.Count);
        }
    }
}