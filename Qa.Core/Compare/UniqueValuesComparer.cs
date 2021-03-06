﻿using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class UniqueValuesComparer
    {
        public UniqueValueSet Compare(IList<RawReportField> rawFields)
        {
            RawReportField previous = null;
            var set = new UniqueValueSet();
            foreach (var current in rawFields)
            {
                set.Add(compare(current.SelectedUniqueValues, previous?.SelectedUniqueValues));
                previous = current;
            }
            var sort = rawFields.First().Description.Sort;
            fillKeys(set, sort);

            return set;
        }

        private void fillKeys(UniqueValueSet set, SortType sort)
        {
            var keys = set.Lists
                .SelectMany(x => x.Values.Select(l => l.Value))
                .Distinct();

            if (sort == SortType.Numeric)
            {
                keys = keys.OrderBy(x => x, new NumericComparer());
            }
            else
            {
                keys = keys.OrderBy(x => x);
            }

            set.Keys = keys.ToList();
        }

        public List<CompareNumber> CompareCounts(IList<RawReportField> rawFields)
        {
            RawReportField previous = null;
            var set = new List<CompareNumber>();
            foreach (var current in rawFields)
            {
                set.Add(new CompareNumber(current.UniqueValuesCount, previous?.UniqueValuesCount));
                previous = current;
            }
            return set;
        }

        private UniqueValueList compare(Dictionary<string, int> current, Dictionary<string, int> previous)
        {
            var row = new UniqueValueList();
            foreach (var pair in current)
            {
                int? prevCount = null;
                if (previous != null && previous.ContainsKey(pair.Key))
                {
                    prevCount = previous[pair.Key];
                }

                row.Add(new UniqueValue { Value = pair.Key, Count = new CompareNumber(pair.Value, prevCount) });
            }
            return row;
        }
    }
}
