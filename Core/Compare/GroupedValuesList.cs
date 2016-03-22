using System.Collections.Generic;
using System.Linq;

namespace Qa.Core.Compare
{
    public class GroupedValuesList
    {
        public List<UniqueValue> Values { get; set; }

        public void Add(UniqueValue value)
        {
            Values.Add(value);
        }

        public GroupedValuesList()
        {
            Values = new List<UniqueValue>();
        }

        public double GetCurrent(string key)
        {
            var value = Values.FirstOrDefault(x => x.Value == key);
            if (value == null)
            {
                return 0;
            }
            return value.Count.Current;
        }

        public double? GetChange(string key)
        {
            var value = Values.FirstOrDefault(x => x.Value == key);
            if (value == null)
            {
                return 0;
            }
            return value.Count.Change;
        }
    }
}