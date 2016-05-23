using Qa.Core.Structure;

namespace Qa.Core.Translates
{
    public interface ITranslator
    {
        string GetTranslate(string key, QaField field);
    }
}