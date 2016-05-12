using System.Collections.Generic;
using Q2.Core.Excel;
using Q2.Core.Structure;

namespace Q2.Core.Compare
{
    public class NumberField : QaField
    {
        public NumberField(FieldPack pack)
            :base(pack.Field)
        {
            Numbers = pack.Numbers;
            Title = pack.Field.Title;
        }

        public List<CompareNumber> Numbers { get; set; }

        public TypedValue GetCurrent(FileInformation report)
        {
            return new TypedValue(Numbers[report.Index].Current, NumberFormat);
        }

        public TypedValue GetChange(FileInformation report)
        {
            return new TypedValue(Numbers[report.Index].Change, NumberFormat.Percent);
        }
    }
}