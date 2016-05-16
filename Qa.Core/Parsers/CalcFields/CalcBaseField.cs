using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public abstract class CalcBaseField
    {
        public QaField Field { get; private set; }

        protected CalcBaseField(QaField field)
        {
            Field = field;
        }
    }
}