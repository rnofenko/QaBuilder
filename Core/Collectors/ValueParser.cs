using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Collectors
{
    public class ValueParser : IDisposable
    {
        private readonly List<ParseField> _fields;
        private readonly List<int> _fieldIndexes = new List<int>();

        public int RowsCount { get; private set; }

        public ValueParser(IEnumerable<FieldDescription> fields)
        {
            _fields = fields.Select(x => new ParseField(x)).ToList();

            for (var index = 0; index < _fields.Count; index++)
            {
                if (_fields[index].Calculation.Type != CalculationType.None)
                {
                    _fieldIndexes.Add(index);
                }
            }
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
            foreach (var index in _fieldIndexes)
            {
                var field = _fields[index];
                var value = parts[index];
                var calculation = field.Calculation;
                
                if (calculation.Type == CalculationType.Count)
                {
                    if (calculation.Group)
                    {
                        try
                        {
                            field.GroupedNumbers[value]++;
                        }
                        catch
                        {
                            field.GroupedNumbers.Add(value, 1);
                        }
                    }
                    else
                    {
                        field.Number += 1;
                    }
                }
                else if (calculation.Type == CalculationType.CountUnique)
                {
                    if (calculation.Group)
                    {
                        HashSet<string> set;
                        if (!field.GroupedUniqueValues.TryGetValue(value, out set))
                        {
                            set = new HashSet<string>();
                            field.GroupedUniqueValues.Add(value, set);
                        }
                        
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
                else if (calculation.Type == CalculationType.Sum || calculation.Type == CalculationType.Average)
                {
                    if (calculation.Group)
                    {
                        var parsed = parseNumeric(parts[field.Calculation.FieldIndex]);
                        try
                        {
                            field.GroupedNumbers[value] += parsed;
                        }
                        catch
                        {
                            field.GroupedNumbers.Add(value, parsed);
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
            if (value.Length == 0)
            {
                return 0;
            }

            try
            {
                return double.Parse(value);
            }
            catch
            {
                throw new InvalidOperationException(string.Format("[{0}] is not a numeric format.", value));
            }
        }

        public void Dispose()
        {
            _fields.Clear();
        }
    }
}