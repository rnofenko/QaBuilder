using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Collectors
{
    public class ValueParser : IDisposable
    {
        private readonly List<ParseField> _fields;

        public int RowsCount { get; private set; }

        public ValueParser(IEnumerable<FieldDescription> fields)
        {
            _fields = fields.Select(x => new ParseField(x)).ToList();
        }

        public List<RawReportField> GetResultFields()
        {
            _fields
                .Where(x => x.Description.Calculation.Type == CalculationType.Average)
                .ToList()
                .ForEach(x =>
                {
                    x.Sum = x.Sum/RowsCount;
                });

            var resultFields = _fields.Select(x => new RawReportField(x)).ToList();

            Dispose();

            return resultFields;
        }

        public void Parse(string[] parts)
        {
            RowsCount++;
            for (var i = 0; i < parts.Length; i++)
            {
                var field = _fields[i];
                var value = parts[i];
                
                if (field.SelectUniqueValues)
                {
                    if (!field.SelectedUniqueValues.ContainsKey(value))
                    {
                        field.SelectedUniqueValues.Add(value, 1);
                    }
                    else
                    {
                        field.SelectedUniqueValues[value]++;
                    }
                }
                else if (field.Calculation.Type == CalculationType.CountUnique)
                {
                    if (!field.CountedUniqueValues.Contains(value))
                    {
                        field.CountedUniqueValues.Add(value);
                    }
                }
                else if (field.Calculation.Type == CalculationType.Sum)
                {
                    if (field.Calculation.Group)
                    {
                        var parsed = double.Parse(parts[field.Calculation.FieldIndex]);
                        if (!field.GroupedSum.ContainsKey(value))
                        {
                            field.GroupedSum.Add(value, parsed);
                        }
                        else
                        {
                            field.GroupedSum[value] += parsed;
                        }
                    }
                    else
                    {
                        field.Sum += double.Parse(value);
                    }
                }
            }
        }

        public void Dispose()
        {
            _fields.Clear();
        }
    }
}
