using System.Collections.Generic;
using Qa.Collectors;
using Qa.Core.Structure;

namespace Qa.Sbpm.Collectors
{
    public class RawReport
    {
        public string Path { get; set; }

        public string Error { get; set; }
        
        public FileStructure Structure { get; set; }

        public int FieldsCount { get; set; }
        
        public int SubReportIndex { get; set; }

        public RawSubReport Summary { get; set; }

        public Dictionary<string, RawSubReport> SubReports { get; set; }

        public RawReport()
        {
            SubReports = new Dictionary<string, RawSubReport>();
        }

        public RawSubReport GetSubReport(string[] values)
        {
            if (SubReportIndex < 0)
            {
                return Summary;
            }
            return GetSubReport(values[SubReportIndex]);
        }

        public RawSubReport GetSubReport(string key)
        {
            if (!SubReports.ContainsKey(key))
            {
                SubReports.Add(key, new RawSubReport(Summary));
            }
            return SubReports[key];
        }
    }
}