using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Compare
{
    public class GroupedValuesSet
    {
        private readonly QaField _field;

        public List<GroupedValuesList> Lists { get; set; }

        public List<string> Keys { get; set; }

        public GroupedValuesSet(QaField field)
        {
            _field = field;
            Lists = new List<GroupedValuesList>();
        }

        public void Add(GroupedValuesList row)
        {
            row.Field = _field;
            Lists.Add(row);
        }
    }
}