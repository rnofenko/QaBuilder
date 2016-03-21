using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Novantas.SaleScape.Dr.Collectors
{
    public class RawReport
    {
        public string Path { get; set; }

        public FileStructure Structure { get; set; }

        public int FieldsCount { get; set; }

        public int RowsCount { get; set; }

        public List<RawReportField> Fields { get; set; }
    }
}