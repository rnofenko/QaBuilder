using System.Collections.Generic;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class NumberField : BaseField
    {
        public NumberField(FieldPack pack)
            : base(pack.Description)
        {
            Numbers = pack.Numbers;
        }

        public List<CompareNumber> Numbers { get; set; }

        public TypedValue GetCurrent(FileInformation report)
        {
            return new TypedValue(Numbers[report.Index].Current, Description.NumberFormat);
        }

        public TypedValue GetChange(FileInformation report)
        {
            return new TypedValue(Numbers[report.Index].Change, NumberFormat.Percent);
        }
    }
}