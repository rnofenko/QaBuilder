using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Sbpm.Collectors
{
    public class RawSubReport
    {
        public int RowsCount { get; set; }

        public List<RawReportField> Fields { get; set; }
        
        public RawSubReport(IEnumerable<FieldDescription> fields)
        {
            Fields = fields.Select(x => new RawReportField(x)).ToList();
        }

        public RawSubReport(List<RawReportField> fields)
        {
            Fields = fields;
        }
    }
}