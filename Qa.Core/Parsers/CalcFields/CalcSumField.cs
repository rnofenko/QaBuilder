using System.Collections.Generic;
using Q2.Core.Collectors;
using Q2.Core.Collectors.CalcFields;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcSumField : CalcBaseField, ICalculationField
    {
        private double _sum;
        private readonly int _index;
        private readonly ExpressionFilter _filter;

        public CalcSumField(QaField field, List<Field> sourceFields) : base(field)
        {
            _index = field.FieldIndex;
            _filter = new ExpressionFilter(field.FilterExpression, sourceFields);
        }

        public void Calc(string[] parts)
        {
            if (!_filter.Match(parts))
            {
                return;
            }

            var value = parts[_index];
            _sum += NumberParser.Parse(value);
        }

        public double GetSingleResult()
        {
            return _sum;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return null;
        }
    }
}