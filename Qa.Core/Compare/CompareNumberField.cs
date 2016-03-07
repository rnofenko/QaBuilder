using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Compares
{
    public class CompareNumberField : CompareNumber
    {
        private readonly FieldDescription _description;

        public DType Type => _description.Type;

        public string Name => _description.Name;

        public string Title => _description.Title ?? _description.Name;

        public List<UniqueValue> UniqueValues { get; set; }

        public CompareNumberField(RawReportField current, RawReportField previous, List<UniqueValue> unique)
            :base(current.Sum, previous?.Sum)
        {
            _description = current.Description;
            UniqueValues = unique;
        }
    }
}