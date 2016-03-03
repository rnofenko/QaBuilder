using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;
using Qa.Structure;

namespace Qa.Collectors
{
    public class RawSubReport
    {
        public int RowsCount { get; set; }

        public List<RawReportField> Fields { get; set; }
        
        public RawSubReport(IEnumerable<FieldDescription> fields)
        {
            Fields = fields.Select(x => new RawReportField(x)).ToList();
        }

        public RawSubReport(RawSubReport summary)
        {
            Fields = summary.Fields.Select(x => new RawReportField(x)).ToList();
        }
    }
}