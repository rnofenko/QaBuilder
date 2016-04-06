using System.Collections.Generic;
using System.Linq;
using Qa.Core.Collectors;
using Qa.Core.Structure;

namespace Qa.Core.Transforms
{
    public class BinCombiner
    {
        public void Combine(List<RawReport> rawReports)
        {
            foreach (var rawReport in rawReports)
            {
                foreach (var field in rawReport.Fields.Where(x=>x.Description.Calculation.Group && x.Description.Bins.IsNotEmpty()))
                {
                    if (field.Type == DType.Numeric)
                    {
                        field.GroupedNumbers = fillNumericBins(field);
                    }
                    else
                    {
                        field.GroupedNumbers = fillStringBins(field);
                    }
                }
            }
        }

        private Dictionary<string, double> fillStringBins(RawReportField field)
        {
            var ranges = field.Description.Bins;
            var bins = new Dictionary<string, double>();
            foreach (var old in field.GroupedNumbers)
            {
                var range = ranges.First(x => string.CompareOrdinal(old.Key, x.From) >= 0 && string.CompareOrdinal(old.Key, x.To) <= 0);
                if (!bins.ContainsKey(range.Name))
                {
                    bins.Add(range.Name, 0d);
                }
                bins[range.Name] += old.Value;
            }
            return bins;
        }

        private Dictionary<string, double> fillNumericBins(RawReportField field)
        {
            var ranges = field.Description.Bins.Select(x => x.ToNumeric()).ToList();
            var bins = new Dictionary<string, double>();
            foreach (var old in field.GroupedNumbers)
            {
                NumericBinRange range;
                double key;
                if (double.TryParse(old.Key, out key))
                {
                    range = ranges.First(x => key >= x.From && key <= x.To);
                }
                else
                {
                    range = ranges.First(x => x.From == null);
                }

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
    }
}