using System;
using System.Collections.Generic;
using System.Diagnostics;
using Qa.Core.Collectors;
using Qa.Core.Structure;
using Xunit;

namespace Qa.Tests
{
    public class PerformanceParserTests
    {
        [Fact]
        public void PerformanceLineParser()
        {
            var parser = new LineParser(",", "\"");
            var w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                parser.Parse("apple,juice,\"pineapple\",apple,juice,\"pineapple\",apple,juice,\"pineapple\",apple,juice,\"pineapple\",apple,juice,\"pineapple\"");
            }
            w.Stop();
            Assert.Equal(new TimeSpan(0, 0, 0, 0), w.Elapsed);
        }

        [Fact]
        public void PerformanceValueParser()
        {
            var fields = new List<FieldDescription>
            {
                new FieldDescription {Type = DType.Numeric, Calculation = new CalculationDescription {Type = CalculationType.Sum}},
                new FieldDescription {Type = DType.String, Calculation = new CalculationDescription()},
                new FieldDescription {Type = DType.Numeric, Calculation = new CalculationDescription {Type = CalculationType.CountUnique, Group = true}},
                new FieldDescription {Type = DType.Numeric, Calculation = new CalculationDescription {Type = CalculationType.Count, Group = true}},
                new FieldDescription {Type = DType.Numeric, Calculation = new CalculationDescription {Type = CalculationType.Average, Group = true}},
                new FieldDescription {Type = DType.Numeric, Calculation = new CalculationDescription {Type = CalculationType.None}},
                new FieldDescription {Type = DType.Numeric, Calculation = new CalculationDescription {Type = CalculationType.None}}
            };
            var valueParser = new ValueParser(fields);
            var parser = new LineParser(",", "\"");

            var w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                var parts = parser.Parse("12.34,apple,\"33.13\",45,tree,juice");
                valueParser.Parse(parts);
            }
            w.Stop();
            Assert.Equal(new TimeSpan(0, 0, 0, 0), w.Elapsed);
        }
    }
}
