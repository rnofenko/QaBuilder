using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class FourMonthsValueComparer : IValueComparer
    {
        public GroupedValuesSet Compare(IEnumerable<Dictionary<string, double>> values, QaField field)
        {
            var list = values.ToList();
            var set = new GroupedValuesSet(field);
            var current = list.First();
            set.Add(compare(current, null));
            foreach (var previous in list.Skip(1))
            {
                set.Add(compare(current, previous));
            }
            fillKeys(set);

            return set;
        }

        public List<CompareNumber> Compare(IEnumerable<double> values)
        {
            var list = values.ToList();
            var current = list.First();
            
            var numbers = new List<CompareNumber> { new CompareNumber(current, null) };
            numbers.AddRange(list.Skip(1).Select(previous => new CompareNumber(current, previous)));
            return numbers;
        }

        private void fillKeys(GroupedValuesSet set)
        {
            set.Keys = set.Lists
                .SelectMany(x => x.Values.Select(l => l.Key))
                .Distinct()
                .ToList();
        }

        private GroupedValuesList compare(Dictionary<string, double> current, Dictionary<string, double> previous)
        {
            var row = new GroupedValuesList();
            foreach (var pair in current)
            {
                double? prevCount = null;
                if (previous != null && previous.ContainsKey(pair.Key))
                {
                    prevCount = previous[pair.Key];
                }

                row.Add(new KeyNumberPair { Key = pair.Key, Count = new CompareNumber(pair.Value, prevCount) });
            }
            return row;
        }
    }
}