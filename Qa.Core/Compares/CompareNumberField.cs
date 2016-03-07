using Qa.Core.Structure;

namespace Qa.Core.Compares
{
    public class CompareNumberField : CompareNumber
    {
        private readonly FieldDescription _description;

        public DType Type => _description.Type;

        public string Name => _description.Name;

        public string Title => _description.Title ?? _description.Name;

        public CompareNumberField(RawReportField current, RawReportField previous)
            :base(current.Sum, previous?.Sum)
        {
            _description = current.Description;
        }
    }
}