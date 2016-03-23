using System.Collections.Generic;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Novantas.SaleScape.Dr.Compare
{
    public class GroupedSumField : BaseField
    {
        public GroupedSumField(FieldPack pack)
            :base(pack.Description)
        {
            Keys = pack.GroupedSumNumbers.Keys;
            ValueLists = pack.GroupedSumNumbers.Lists;
        }

        public List<GroupedValuesList> ValueLists { get; set; }

        public List<string> Keys { get; set; }

        public static bool IsConvertable(FieldPack pack)
        {
            return pack.IsGrouped;
        }
    }
}