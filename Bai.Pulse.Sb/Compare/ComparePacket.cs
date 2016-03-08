using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;
using Qa.Sbpm.Compare;

namespace Qa.Bai.Sbp.Compare
{
    public class ComparePacket
    {
        public FileStructure Strucure { get; set; }

        public List<CompareReport> Reports { get; set; }

        public List<string> States { get; set; }

        public ComparePacket()
        {
            Reports = new List<CompareReport>();
        }

        public List<CompareSubReport> GetTransformedSubReports(string transformation, string state)
        {
            return Reports.Select(x => x.TransformReports[transformation].GetSubReport(state)).ToList();
        }
    }
}