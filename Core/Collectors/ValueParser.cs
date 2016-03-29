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
                if (field.Type == DType.Numeric)
                {
                    if (value.IsNotEmpty())
                    {
                        var parsed = double.Parse(value);                        
                        if (field.Description.Calculation.GroupByIndex >= 0)
                        {
                            var key = parts[field.Description.Calculation.GroupByIndex];
                            if (!field.GroupedSum.ContainsKey(key))
                            {
                                field.GroupedSum.Add(key, parsed);
                            }
                            else
                            {
                                field.GroupedSum[key] += parsed;
                            }
                        }
                        else
                        {
                            field.Sum += parsed;
                        }
                    }
                }
                
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
            }
        }

        public void Dispose()
        {
            _fields.Clear();
        }
    }
}
