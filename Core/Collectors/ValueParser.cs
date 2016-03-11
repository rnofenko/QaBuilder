using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Collectors
{
    public class ValueParser : IDisposable
    {
        public List<ParseField> Fields { get; }

        public int RowsCount { get; private set; }

        public ValueParser(IEnumerable<FieldDescription> fields)
        {
            Fields = fields.Select(x => new ParseField(x)).ToList();
        }

        public void Parse(string[] parts)
        {
            RowsCount++;

            for (var i = 0; i < parts.Length; i++)
            {
                var field = Fields[i];
                var value = parts[i];
                if (field.Type == DType.Double)
                {
                    if (value.IsNotEmpty())
                    {
                        field.Sum += double.Parse(value);
                    }
                }
                if (field.Type == DType.Money)
                {
                    if (value.IsNotEmpty())
                    {
                        field.Sum += double.Parse(value);
                    }
                }
                else if (field.Type == DType.Int)
                {
                    if (value.IsNotEmpty())
                    {
                        field.Sum += double.Parse(value);
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
                else if (field.CountUniqueValues)
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
            Fields.Clear();
        }
    }
}
