using System.Collections.Generic;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class UniqueCountField : BaseField
    {
        public UniqueCountField(FieldPack pack)
            : base(pack.Description)
        {
            Counts = pack.Numbers;
        }

        public List<CompareNumber> Counts { get; set; }

        public TypedValue GetCurrent(CompareReport report)
        {
            return Counts[report.Index].CurrentAsInteger;
        }

        public TypedValue GetChange(CompareReport report)
        {
            return Counts[report.Index].ChangeAsPercent;
        }

        public static bool IsConvertable(FieldPack pack)
        {
            return pack.Description.Calculation.Type == CalculationType.CountUnique;
        }
    }
}