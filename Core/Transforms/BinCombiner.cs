using System.Collections.Generic;
using System.Linq;
using Qa.Core.Collectors;

namespace Qa.Core.Transforms
{
    public class BinCombiner
    {
        public void Combine(List<RawReport> rawReports)
        {
            foreach (var rawReport in rawReports)
            {
                foreach (var field in rawReport.Fields.Where(x=>x.Description.SelectUniqueValues && x.Description.Bins.IsNotEmpty()))
                {
                    var ranges = field.Description.Bins;
                    var bins = new Dictionary<string, double>();
                    foreach (var old in field.SelectedUniqueValues)
                    {
                        var range = ranges.First(x => string.CompareOrdinal(old.Key, x.From) >= 0 && string.CompareOrdinal(old.Key, x.To) <= 0);
                        if (!bins.ContainsKey(range.Name))
                        {
                            bins.Add(range.Name, 0d);
                        }
                        bins[range.Name] += old.Value;
                    }
                    field.SelectedUniqueValues = bins;
                }
            }
        }
    }
}