using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Collectors
{
    public class ValueParser
    {
        public void Parse(string[] parts, List<RawReportField> fields)
        {
            for (var i = 0; i < parts.Length; i++)
            {
                var field = fields[i];
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

                if (field.CalcUnique)
                {
                    if (!field.UniqueValues.ContainsKey(value))
                    {
                        field.UniqueValues.Add(value, 1);
                    }
                    else
                    {
                        field.UniqueValues[value]++;
                    }
                }
            }
        }
    }
}
