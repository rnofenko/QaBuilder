namespace Qa.Core.Parsers.Filters
{
    public class AlwaysTrueExpressionFilter : IExpressionFilter
    {
        public bool Match(string[] parts)
        {
            return true;
        }
    }
}