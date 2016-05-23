using System.Collections.Generic;
using Qa.Core.Parsers.Filters;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcSumField : CalcBaseField, ICalculationField
    {
        private double _sum;
        private readonly int _index;
        private readonly IExpressionFilter _filter;

        public CalcSumField(QaField field, List<Field> sourceFields) : base(field)
        {
            _index = field.FieldIndex;
            _filter = ExpressionFilterFactory.Create(field.FilterExpression, sourceFields);
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