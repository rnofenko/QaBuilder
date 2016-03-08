using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class CompareField
    {
        private readonly FieldDescription _description;

        public DType Type => _description.Type;

        public string Name => _description.Name;

        public string Title => _description.Title ?? _description.Name;

        public CompareNumber Number { get; set; }

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
            Number = new CompareNumber(current.Sum, previous?.Sum);
        }

        public TypedValue GetCurrent()
        {
            return new TypedValue(Number.Current, Type);
        }

        public TypedValue GetChange()
        {
            return new TypedValue(Number.Change, DType.Percent);
        }
    }
}