using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class GroupedSumField : BaseField
    {
        public GroupedSumField(FieldPack pack)
            :base(pack.Description)
        {
            Keys = pack.GroupedSumNumbers.Keys;
            ValueLists = pack.GroupedSumNumbers.Lists;
        }

        public FieldDescription GroupByField => Description.Calculation.GroupByField;

        public List<GroupedValuesList> ValueLists { get; set; }

        public List<string> Keys { get; set; }

        public static bool IsConvertable(FieldPack pack)
        {
            return pack.Description.Calculation.IsGrouped();
        }
    }
}