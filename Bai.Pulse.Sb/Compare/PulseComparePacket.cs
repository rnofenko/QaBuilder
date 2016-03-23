using System.Collections.Generic;
using System.Linq;
using Qa.Bai.Sbp.Compare;
using Qa.Core.Structure;

namespace Qa.Bai.Pulse.Sb.Compare
{
    public class PulseComparePacket
    {
        public FileStructure Strucure { get; set; }

        public List<PulseCompareReport> Reports { get; set; }

        public List<string> States { get; set; }

        public PulseComparePacket()
        {
            Reports = new List<PulseCompareReport>();
        }

        public List<CompareSubReport> GetTransformedSubReports(string transformation, string state)
        {
            return Reports.Select(x => x.TransformReports[transformation].GetSubReport(state)).ToList();
        }
    }
}