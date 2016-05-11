using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Compare
{
    public class FieldPack
    {
        public QaField Field { get; private set; }

        public GroupedValuesSet GroupedNumbers { get; set; }

        public List<CompareNumber> Numbers { get; set; }
        
        public string FileName { get; set; }

        public FieldPack(QaField qaField)
        {
            Field = qaField;
            Numbers =  new List<CompareNumber>();
        }

        public CompareField GetNumberField(int periodId)
        {
            return new CompareField(Field, Numbers[periodId]);
        }
    }
}