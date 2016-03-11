using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Bai.Pulse.Sb.Collectors
{
    public class RawSubReport
    {
        public int RowsCount { get; set; }

        public List<RawReportField> Fields { get; set; }
    }
}