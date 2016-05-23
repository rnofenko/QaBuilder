namespace Qa.Core.Parsers.Filters
{
    public interface IExpressionFilter
    {
        bool Match(string[] parts);
    }
}