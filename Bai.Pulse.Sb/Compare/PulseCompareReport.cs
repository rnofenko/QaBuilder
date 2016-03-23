using System.Collections.Generic;
using System.Linq;

namespace Qa.Bai.Pulse.Sb.Compare
{
    public class PulseCompareReport
    {
        public List<CompareSubReport> SubReports { get; set; }

        public Dictionary<string, PulseCompareReport> TransformReports { get; set; }

        public PulseCompareReport()
        {
            SubReports = new List<CompareSubReport>();
            TransformReports = new Dictionary<string, PulseCompareReport>();
        }

        public CompareSubReport GetSubReport(string key)
        {
            return SubReports.FirstOrDefault(x => x.State == key);
        }
    }
}