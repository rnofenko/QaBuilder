namespace Qa.Core.Parsers
{
    public interface ICsvParser
    {
        string[] Parse(string line);
    }
}