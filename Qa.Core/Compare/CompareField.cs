using Q2.Core.Excel;
using Q2.Core.Structure;
using Qa.Core.Structure;

namespace Q2.Core.Compare
{
    public class CompareField
    {
        private readonly QaField _qaField;

        public string Title
        {
            get { return _qaField.Title; }
        }

        public CompareNumber Number { get; private set; }

        public CompareField(QaField qaField, CompareNumber number)
        {
            _qaField = qaField;
            Number = number;
        }

        public TypedValue GetCurrent()
        {
            return new TypedValue(Number.Current, _qaField.NumberFormat);
        }

        public TypedValue GetChange()
        {
            if (_qaField.NumberFormat == NumberFormat.Rate)
            {
                return new TypedValue(Number.AbsoluteChange, NumberFormat.Rate);
            }

            return new TypedValue(Number.Change, NumberFormat.Percent);
        }
    }
}