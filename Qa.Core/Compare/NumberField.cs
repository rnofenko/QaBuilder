using System.Collections.Generic;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
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

        public TypedValue GetPercentChange(FileInformation report)
        {
            var number = Numbers[report.Index];
            if (NumberFormat == NumberFormat.Rate)
            {
                return new TypedValue(number.AbsChange, NumberFormat.Rate);
            }
            return new TypedValue(number.PercentChange, NumberFormat.Percent);
        }

        public TypedValue GetAbsChange(FileInformation report)
        {
            var number = Numbers[report.Index];
            return new TypedValue(number.AbsChange, NumberFormat);
        }
    }
}