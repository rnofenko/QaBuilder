using System.Collections.Generic;
using Q2.Core.Collectors;
using Q2.Core.Collectors.CalcFields;
using Q2.Core.Structure;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcGroupedSumField : CalcBaseField, ICalculationField
    {
        private readonly int _index;
        private readonly Dictionary<string, double> _groupedNumbers;

        public CalcGroupedSumField(QaField field)
            :base(field)
        {
            _index = field.FieldIndex;
            _groupedNumbers = new Dictionary<string, double>();
        }

        public void Calc(string[] parts)
        {
            var value = parts[_index];            
            var parsed = NumberParser.Parse(value);
            try
            {
                _groupedNumbers[value] += parsed;
            }
            catch
            {
                _groupedNumbers.Add(value, parsed);
            }
        }

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