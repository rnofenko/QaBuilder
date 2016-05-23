using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.Filters
{
    public class ExpressionFilterFactory
    {
        public static IExpressionFilter Create(string expression, List<Field> sourceFields)
        {
            var equalFilter = new EqualExpressionFilter();
            if (equalFilter.ParseExpression(expression, sourceFields))
            {
                return equalFilter;
            }
            var inFilter = new InExpressionFilter();
            if (inFilter.ParseExpression(expression, sourceFields))
            {
                return inFilter;
            }
            return new AlwaysTrueExpressionFilter();
        }
    }
}