using Q2.Core.Structure;
using Qa.Core.Structure;

namespace Q2.Core.Collectors.CalcFields
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