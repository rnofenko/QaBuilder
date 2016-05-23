using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.Filters
{
    public class EqualExpressionFilter: IExpressionFilter
    {
        private int _filterFieldIndex;
        private string _filterValue;

        public bool Match(string[] parts)
        {
            return parts[_filterFieldIndex] == _filterValue;
        }

        public bool ParseExpression(string expression, List<Field> sourceFields)
        {
            if (expression.IsEmpty())
            {
                return false;
            }
            var parts = expression.Split('=');
            if (parts.Length != 2)
            {
                return false;
            }
            _filterFieldIndex = sourceFields.FindIndex(x => x.Name == parts[0].Trim());
            _filterValue = parts[1].Trim();
            return _filterFieldIndex >= 0;
        }
    }
}