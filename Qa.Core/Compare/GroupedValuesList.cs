using System.Collections.Generic;
using System.Linq;
using Q2.Core.Excel;
using Q2.Core.Structure;
using Qa.Core.Structure;

namespace Q2.Core.Compare
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
            var value = Values.FirstOrDefault(x => x.Value == key);
            if (value == null)
            {
                return new TypedValue(0, NumberFormat.Integer);
            }
            return new TypedValue(value.Count.Current, NumberFormat.Integer);
        }

        public TypedValue GetChange(string key)
        {
            var value = Values.FirstOrDefault(x => x.Value == key);
            if (value == null)
            {
                return new TypedValue(0, NumberFormat.Percent);
            }
            return new TypedValue(value.Count.Change, NumberFormat.Percent);
        }
    }
}