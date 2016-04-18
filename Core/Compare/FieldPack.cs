using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class FieldPack : BaseField
    {
        public DType Type
        {
            get { return Description.Type; }
        }

        public string Name
        {
            get { return Description.Name; }
        }

        public bool IsGrouped
        {
            get { return Description.Calculation.Group; }
        }

        public GroupedValuesSet GroupedNumbers { get; set; }

        public List<CompareNumber> Numbers { get; set; }
        
        public string FileName { get; set; }

        public FieldPack(FieldDescription fieldDescription)
            :base(fieldDescription)
        {
            Numbers =  new List<CompareNumber>();
        }

        public CompareField GetNumberField(int periodId)
        {
            return new CompareField(Description, Numbers[periodId]);
        }
    }
}