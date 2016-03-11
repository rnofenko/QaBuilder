using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Bai.Benchmark.Dp.Collectors
{
    public class RawReport
    {
        public string Path { get; set; }

        public string Error { get; set; }

        public FileStructure Structure { get; set; }

        public int FieldsCount { get; set; }

        public int RowsCount { get; set; }

        public List<RawReportField> Fields { get; set; }
    }
}