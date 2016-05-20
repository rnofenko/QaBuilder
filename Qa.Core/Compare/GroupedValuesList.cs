using System.Collections.Generic;
using System.Linq;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class GroupedValuesList
    {
        public List<KeyNumberPair> Values { get; set; }

        public QaField Field { get; set; }

        public void Add(KeyNumberPair value)
        {
            Values.Add(value);
        }

        public GroupedValuesList()
        {
            Values = new List<KeyNumberPair>();
        }

        public TypedValue GetCurrent(string key)
        {
            var value = Values.FirstOrDefault(x => x.Key == key);
            if (value == null)
            {
                return new TypedValue(0, NumberFormat.Integer);
            }
            return new TypedValue(value.Count.Current, NumberFormat.Integer);
        }

        public TypedValue GetChange(string key)
        {
            var value = Values.FirstOrDefault(x => x.Key == key);
            if (value == null)
            {
                return new TypedValue(0, NumberFormat.Percent);
            }
            return new TypedValue(value.Count.Change, NumberFormat.Percent);
        }
    }
}