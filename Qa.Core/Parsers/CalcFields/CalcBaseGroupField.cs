using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public abstract class CalcBaseGroupField: CalcBaseField
    {
        protected const string SEPARATOR = "||";
        private readonly int[] _indexes;

        protected CalcBaseGroupField(QaField field) : base(field)
        {
            _indexes = field.GroupByIndexes;
        }

        protected string GetKey(string[] parts)
        {
            return string.Join(SEPARATOR, _indexes.Select(x => parts[x]));
        }
    }
}