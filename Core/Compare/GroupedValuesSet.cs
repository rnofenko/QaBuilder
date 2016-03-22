using System.Collections.Generic;

namespace Qa.Core.Compare
{
    public class GroupedValuesSet
    {
        public List<GroupedValuesList> Lists { get; set; }

        public List<string> Keys { get; set; }

        public GroupedValuesSet()
        {
            Lists = new List<GroupedValuesList>();
        }

        public void Add(GroupedValuesList row)
        {
            Lists.Add(row);
        }
    }
}