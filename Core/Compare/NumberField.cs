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

        public TypedValue GetCurrent(CompareReport report)
        {
            return Numbers[report.Index].CurrentAsInteger;
        }

        public TypedValue GetChange(CompareReport report)
        {
            return Numbers[report.Index].ChangeAsPercent;
        }

        public static bool IsConvertable(FieldPack pack)
        {
            return pack.Description.Calculation.Type == CalculationType.CountUnique;
        }
    }
}