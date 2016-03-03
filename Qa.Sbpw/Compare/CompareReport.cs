using System.Collections.Generic;
using System.Linq;

namespace Qa.Sbpm.Compare
{
    public class CompareReport
    {
        public CompareSubReport Summary { get; set; }

        public List<CompareSubReport> SubReports { get; set; }

        public CompareReport()
        {
            SubReports = new List<CompareSubReport>();
        }

        public CompareSubReport GetSubReport(string key)
        {
            return SubReports.FirstOrDefault(x => x.Key == key);
        }
    }
}