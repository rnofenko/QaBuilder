using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class UniqueCountField : BaseField
    {
        public UniqueCountField(FieldPack pack)
            : base(pack.Description)
        {
            Counts = pack.UniqueValueCounts;
        }

        public List<CompareNumber> Counts { get; set; }

        public static bool IsConvertable(FieldPack pack)
        {
            return pack.Description.CountUniqueValues;
        }
    }
}