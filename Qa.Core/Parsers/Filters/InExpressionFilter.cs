using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.Filters
{
    public class InExpressionFilter : IExpressionFilter
    {
        private int _filterFieldIndex;
        private List<string> _filterValues;

        public bool Match(string[] parts)
        {
            var value = parts[_filterFieldIndex];
            return _filterValues.Contains(value);
        }

        public bool ParseExpression(string expression, List<Field> sourceFields)
        {
            if (expression.IsEmpty())
            {
                return false;
            }
            var parts = expression.Split(new[] {" in "}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return false;
            }
            _filterFieldIndex = sourceFields.FindIndex(x => x.Name == parts[0].Trim());
            var values = parts[1].Trim();
            values = values.Substring(1, values.Length - 2);
            _filterValues = new CsvParser(",", "'").Parse(values).ToList();
            return _filterFieldIndex >= 0 && _filterValues.Any();
        }
    }
}