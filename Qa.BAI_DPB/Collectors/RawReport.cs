using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.BAI_DPB.Collectors
{
    public class RawReport
    {
        public string Path { get; set; }

        public string Error { get; set; }

        public FileStructure Structure { get; set; }

        public int FieldsCount { get; set; }

        public int SubReportIndex { get; set; }

        public List<FieldDescription> Fields { get; set; }

        public Dictionary<string, RawReport> TransformedReports { get; set; }
        public Dictionary<string, RawSubReport> SubReports { get; set; }

        public RawReport(IEnumerable<FieldDescription> fields)
        {
            Fields = fields.ToList();
            SubReports = new Dictionary<string, RawSubReport>();
            TransformedReports = new Dictionary<string, RawReport>();
        }

        public RawSubReport GetSubReport(string[] values)
        {
            return GetSubReport(values[SubReportIndex]);
        }

        public RawSubReport GetSubReport(string state)
        {
            if (!SubReports.ContainsKey(state))
            {
                SubReports.Add(state, new RawSubReport(Fields));
            }
            return SubReports[state];
        }
    }
}