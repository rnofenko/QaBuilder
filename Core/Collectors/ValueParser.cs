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
                    x.Number = x.Number / RowsCount;
                });

            var resultFields = _fields.Select(x => new RawReportField(x)).ToList();
            resultFields.Insert(0, new RawReportField(FieldDescription.RowsCountDescription(), RowsCount));

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
                
                if (field.Calculation.Type == CalculationType.Count)
                {
                    if (field.Calculation.Group)
                    {
                        if (!field.GroupedNumbers.ContainsKey(value))
                        {
                            field.GroupedNumbers.Add(value, 1);
                        }
                        else
                        {
                            field.GroupedNumbers[value]++;
                        }
                    }
                    else
                    {
                        field.Number += 1;
                    }
                }
                else if (field.Calculation.Type == CalculationType.CountUnique)
                {
                    if (field.Calculation.Group)
                    {
                        if (!field.GroupedUniqueValues.ContainsKey(value))
                        {
                            field.GroupedUniqueValues.Add(value, new HashSet<string>());
                        }
                        var set = field.GroupedUniqueValues[value];
                        var fieldValue = parts[field.Calculation.FieldIndex];
                        if (!set.Contains(fieldValue))
                        {
                            set.Add(fieldValue);
                        }
                    }
                    else
                    {
                        if (!field.UniqueValues.Contains(value))
                        {
                            field.UniqueValues.Add(value);
                        }
                    }
                }
                else if (field.Calculation.Type == CalculationType.Sum || field.Calculation.Type == CalculationType.Average)
                {
                    if (field.Calculation.Group)
                    {
                        var parsed = parseNumeric(parts[field.Calculation.FieldIndex]);
                        if (!field.GroupedNumbers.ContainsKey(value))
                        {
                            field.GroupedNumbers.Add(value, parsed);
                        }
                        else
                        {
                            field.GroupedNumbers[value] += parsed;
                        }
                    }
                    else
                    {
                        field.Number += parseNumeric(value);
                    }
                }
            }
        }

        private double parseNumeric(string value)
        {
            double res;
            if (double.TryParse(value, out res))
            {
                return res;
            }
            if (value == "")
            {
                return 0;
            }
            throw new InvalidOperationException($"[{value}] is not a numeric format.");
        }

        public void Dispose()
        {
            _fields.Clear();
        }
    }
}