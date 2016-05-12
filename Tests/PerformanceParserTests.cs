using System;
using System.Diagnostics;
using Qa.Core.Parsers;
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
    }
}
