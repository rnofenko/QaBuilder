using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Calculations;
using Qa.Core.Parsers;
using Qa.Core.Structure;

namespace Qa.Core.Transforms
{
    public class BinCombiner
    {
        public List<ParsedFile> Combine(List<ParsedFile> files)
        {
            foreach (var rawReport in files)
            {
                foreach (var field in rawReport.Fields.Where(x=>x.Field.Bins != null))
                {
                    field.GroupedNumbers = fillBins(field);
                }
            }

            return files;
        }

        private Dictionary<string, double> fillBins(CalculatedField field)
        {
            var settings = field.Field.Bins;
            var bins = new Dictionary<string, double>();
            var total = calcTotal(field.GroupedNumbers, settings);
            foreach (var old in field.GroupedNumbers)
            {
                var range = findRange(old, settings, total);
                if (!range.Hide)
                {
                    if (!bins.ContainsKey(range.Name))
                    {
                        bins.Add(range.Name, 0d);
                    }
                    bins[range.Name] += old.Value;
                }
            }
            return bins;
        }

        private double calcTotal(Dictionary<string, double> numbers, BinSettings settings)
        {
            if (settings.Method != BinMethod.Proportional)
            {
                return 0;
            }

            var total = 0d;
            foreach (var number in numbers)
            {
                var value = settings.Source == BinSource.Key ? NumberParser.SafeParse(number.Key) : number.Value;
                total += Math.Abs(value);
            }

            return total;
        }

        private BinRange findRange(KeyValuePair<string, double> pair, BinSettings settings, double total)
        {
            var ranges = settings.Ranges.OrderBy(x => x.UpTo, new StringsAsNumbersComparer()).ToList();
            var value = settings.Source == BinSource.Key ? pair.Key : pair.Value.ToStr();
            var range = settings.Method == BinMethod.Proportional ? findByProportionalValue(value, ranges, total) : findByAbsoluteValue(value, ranges);
            if (range == null)
            {
                range = ranges.FirstOrDefault(x => x.UpTo == null) ?? ranges.First();
            }
            if (range.SplitToItems)
            {
                return new BinRange {Name = pair.Key};
            }
            return range;
        }

        private BinRange findByAbsoluteValue(string value, List<BinRange> ranges)
        {
            return ranges.FirstOrDefault(x => value.LessOrEqualThan(x.UpTo));
        }

        private BinRange findByProportionalValue(string value, List<BinRange> ranges, double total)
        {
            var number = Math.Abs(NumberParser.SafeParse(value));
            var portion = Calculator.Portion(number, total);

            return ranges.FirstOrDefault(x => portion.LessOrEqualThan(x.UpTo));
        }
    }
}