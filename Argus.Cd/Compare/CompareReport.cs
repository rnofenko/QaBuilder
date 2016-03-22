using System.Collections.Generic;
using Qa.Core.Compare;

namespace Qa.Bai.Benchmark.Sb.Compare
{
    public class CompareReport
    {
        public CompareNumber RowsCount { get; set; }

        public List<CompareField> Numbers { get; set; }

        public string FileName { get; set; }

        public CompareReport()
        {
            Numbers = new List<CompareField>();
        }
    }
}