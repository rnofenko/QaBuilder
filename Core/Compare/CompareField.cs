using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class CompareField
    {
        private readonly FieldDescription _description;

        public string Title
        {
            get { return _description.Title ?? _description.Name; }
        }

        public CompareNumber Number { get; private set; }

        public CompareField(RawReportField current)
        {
            _description = current.Description;
        }

        public CompareField(FieldDescription fieldDescription, CompareNumber number)
        {
            _description = fieldDescription;
            Number = number;
        }

        public CompareField(RawReportField current, RawReportField previous)
        {
            _description = current.Description;
            Number = new CompareNumber(current.Number, previous != null ? previous.Number : (double?)null);
        }

        public TypedValue GetCurrent()
        {
            return new TypedValue(Number.Current, _description.NumberFormat);
        }

        public TypedValue GetChange()
        {
            if (_description.NumberFormat == NumberFormat.Rate)
            {
                return new TypedValue(Number.AbsoluteChange, NumberFormat.Rate);
            }

            return new TypedValue(Number.Change, NumberFormat.Percent);
        }
    }
}