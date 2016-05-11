using System.Collections.Generic;
using Q2.Core.Extensions;
using Q2.Core.Structure;

namespace Q2.Core.Collectors
{
    public class ExpressionFilter
    {
        private bool _alwaysMatch = true;
        private int _filterFieldIndex;
        private string _filterValue;

        public ExpressionFilter(string expression, List<Field> sourceFields)
        {
            parseExpression(expression, sourceFields);
        }

        public bool Match(string[] parts)
        {
            if (_alwaysMatch)
            {
                return true;
            }

            return parts[_filterFieldIndex] == _filterValue;
        }

        private void parseExpression(string expression, List<Field> sourceFields)
        {
            if (expression.IsEmpty())
            {
                return;
            }
            _alwaysMatch = false;
            var parts = expression.Split('=');
            _filterFieldIndex = sourceFields.FindIndex(x => x.Name == parts[0]);
            _filterValue = parts[1];
        }
    }
}