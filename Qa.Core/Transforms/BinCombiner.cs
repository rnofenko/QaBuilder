using System.Collections.Generic;
using System.Linq;
using Qa.Core.Parsers;
using Qa.Core.Structure;

namespace Qa.Core.Transforms
{
    public class BinCombiner
    {
        public void Combine(List<ParsedFile> files)
        {
            foreach (var rawReport in files)
            {
                foreach (var field in rawReport.Fields.Where(x=>x.Field.Bins != null))
                {
                    field.GroupedNumbers = fillBins(field);
                }
            }
        }

        private Dictionary<string, double> fillBins(CalculatedField field)
        {
            var settings = field.Field.Bins;
            var ranges = settings.Ranges;
            var bins = new Dictionary<string, double>();
            foreach (var old in field.GroupedNumbers)
            {
                var fieldValue = settings.Source == BinSource.Key ? old.Key : old.Value.ToStr();

                var range = ranges.FirstOrDefault(x => fieldValue.BiggerOrEqualThan(x.From) && fieldValue.LessOrEqualThan(x.To));
                if (range == null)
                {
                    range = ranges.First(x => x.From == null && x.To == null);
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