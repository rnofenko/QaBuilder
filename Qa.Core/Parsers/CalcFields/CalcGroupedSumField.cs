using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcGroupedSumField : CalcBaseGroupField, ICalculationField
    {
        private readonly Dictionary<string, double> _groupedNumbers;
        private readonly int _index;

        public CalcGroupedSumField(QaField field)
            :base(field)
        {
            _groupedNumbers = new Dictionary<string, double>();
            _index = field.FieldIndex;
        }

        public void Calc(string[] parts)
        {
            var key = GetKey(parts);
            var parsed = NumberParser.Parse(parts[_index]);
            try
            {
                _groupedNumbers[key] += parsed;
            }
            catch
            {
                _groupedNumbers.Add(key, parsed);
            }
        }

        public double GetSingleResult()
        {
            return 0;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return _groupedNumbers.ToDictionary(x => x.Key.Replace(SEPARATOR, " / "), x => x.Value);
        }
    }
}