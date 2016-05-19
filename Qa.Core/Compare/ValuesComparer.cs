using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class ValuesComparer
    {
        public GroupedValuesSet Compare(IEnumerable<Dictionary<string, double>> values, QaField field)
        {
            Dictionary<string, double> previous = null;
            var set = new GroupedValuesSet(field);
            foreach (var current in values)
            {
                set.Add(compare(current, previous));
                previous = current;
            }
            var sort = field.Sort;
            fillKeys(set, sort);

            return set;
        }

        public List<CompareNumber> Compare(IEnumerable<int> values)
        {
            return Compare(values.Select(x => (double) x));
        }

        public List<CompareNumber> Compare(IEnumerable<double> values)
        {
            double? previous = null;
            var numbers = new List<CompareNumber>();
            foreach (var current in values)
            {
                numbers.Add(new CompareNumber(current, previous));
                previous = current;
            }
            return numbers;
        }

        private void fillKeys(GroupedValuesSet set, SortType sort)
        {
            var keys = set.Lists
                .SelectMany(x => x.Values.Select(l => l.Key))
                .Distinct();

            if (sort == SortType.Numeric)
            {
                keys = keys.OrderBy(x => x, new NumericComparer());
            }
            else if (sort == SortType.StringAsNumber)
            {
                keys = keys.OrderBy(x => x, new StringsAsNumbersComparer());
            }
            else
            {
                keys = keys.OrderBy(x => x);
            }

            set.Keys = keys.ToList();
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
