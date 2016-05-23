using System.Collections.Generic;
using System.Linq;
using Qa.Core.Parsers.Filters;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcGroupedSumField : CalcBaseGroupField, ICalculationField
    {
        private readonly Dictionary<string, double> _groupedNumbers;
        private readonly int _index;
        private IExpressionFilter _filter;

        public CalcGroupedSumField(QaField field, List<Field> sourceFields)
            :base(field)
        {
            _groupedNumbers = new Dictionary<string, double>();
            _index = field.FieldIndex;
            _filter = ExpressionFilterFactory.Create(field.FilterExpression, sourceFields);
        }

        public void Calc(string[] parts)
        {
            if (!_filter.Match(parts))
            {
                return;
            }

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