using System.Collections.Generic;
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

        public List<UniqueValue> UniqueValues { get; set; }

        public CompareNumber Number { get; set; }

        public CompareField(RawReportField current, List<UniqueValue> unique)
        {
            _description = current.Description;
            UniqueValues = unique;
        }

        public CompareField(RawReportField current, RawReportField previous, List<UniqueValue> unique)
        {
            _description = current.Description;
            UniqueValues = unique;
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