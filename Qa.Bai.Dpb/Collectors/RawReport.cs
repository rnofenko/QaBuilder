using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Bai.Dpb.Collectors
{
    public class RawReport
    {
        public string Path { get; set; }

        public string Error { get; set; }

        public FileStructure Structure { get; set; }

        public int FieldsCount { get; set; }

        public int RowsCount { get; set; }

        public List<RawReportField> Fields { get; set; }

        public RawReport(IEnumerable<FieldDescription> fields)
        {
            Fields = fields.Select(x => new RawReportField(x)).ToList();
        }
    }
}