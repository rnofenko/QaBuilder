using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class FieldPack : BaseField
    {
        public DType Type => Description.Type;

        public string Name => Description.Name;
        
        public bool SelectUniqueValues => Description.SelectUniqueValues;

        public bool IsGroupedSumField => Description.Calculation.Group;

        public GroupedValuesSet UniqueValues { get; set; }
        public List<CompareNumber> UniqueValueCounts { get; set; }

        public GroupedValuesSet GroupedSumNumbers { get; set; }
        public List<CompareNumber> SumNumbers { get; set; }
        
        public string FileName { get; set; }

        public FieldPack(FieldDescription fieldDescription)
            :base(fieldDescription)
        {
            SumNumbers =  new List<CompareNumber>();
        }

        public CompareField GetNumberField(int periodId)
        {
            return new CompareField(Description, SumNumbers[periodId]);
        }
    }
}