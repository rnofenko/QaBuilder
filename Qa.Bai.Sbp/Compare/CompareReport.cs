using System.Collections.Generic;
using System.Linq;

namespace Qa.Bai.Sbp.Compare
{
    public class CompareReport
    {
        public List<CompareSubReport> SubReports { get; set; }

        public Dictionary<string, CompareReport> TransformReports { get; set; }

        public CompareReport()
        {
            SubReports = new List<CompareSubReport>();
            TransformReports = new Dictionary<string, CompareReport>();
        }

        public CompareSubReport GetSubReport(string key)
        {
            return SubReports.FirstOrDefault(x => x.State == key);
        }
    }
}