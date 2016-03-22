using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class FieldPack
    {
        private readonly FieldDescription _description;

        public DType Type => _description.Type;

        public string Name => _description.Name;

        public string Title => _description.Title ?? _description.Name;

        public bool SelectUniqueValues => _description.SelectUniqueValues;

        public bool CountUniqueValues => _description.CountUniqueValues;

        public bool GroupedSumValues => _description.Calculation != null && _description.Calculation.IsGrouped();

        public GroupedValuesSet UniqueValues { get; set; }
        public List<CompareNumber> UniqueValueCounts { get; set; }

        public GroupedValuesSet GroupedSumNumbers { get; set; }
        public List<CompareNumber> SumNumbers { get; set; }
        
        public string FileName { get; set; }

        public FieldPack(FieldDescription filDescription)
        {
            _description = filDescription;
            SumNumbers =  new List<CompareNumber>();
        }

        public CompareField GetNumberField(int periodId)
        {
            return new CompareField(_description, SumNumbers[periodId]);
        }
    }
}