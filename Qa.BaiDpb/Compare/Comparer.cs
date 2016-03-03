using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.BaiDpb.Collectors;
using Qa.Core.Structure;

namespace Qa.BaiDpb.Compare
{
    public class Comparer
    {
        public List<ComparePacket> Compare(List<RawReport> statistics)
        {
            var packets = statistics
                .GroupBy(x => x.Structure.Name)
                .Select(compare)
                .ToList();
            return packets;
        }

        private ComparePacket compare(IEnumerable<RawReport> rawReports)
        {
            var reports = rawReports.OrderBy(x => x.Path).ToList();
            var first = reports.First();
            var packet = new ComparePacket {Structure = first.Structure};
            
            RawReport previous = null;
            foreach (var report in reports)
            {
                packet.Reports.Add(compare(report, previous));
                previous = report;
            }

            return packet;
        }

        private CompareReport compare(RawReport current, RawReport previous)
        {
            var fileName = Path.GetFileNameWithoutExtension(current.Path);
            return compare(current, previous, fileName);
        }

        private CompareReport compare(RawReport current, RawReport previous, string fileName)
        {
            var result = new CompareReport
            {
                RowsCount = new CompareNumber(current.RowsCount, previous?.RowsCount),
                FileName = fileName
            };

            for (var i = 0; i < current.Fields.Count; i++)
            {
                var fieldCurrent = current.Fields[i];
                RawReportField fieldPrev = null;
                if (previous != null)
                {
                    fieldPrev = previous.Fields[i];
                }

                if (fieldCurrent.Type == DType.Double || fieldCurrent.Type == DType.Int || fieldCurrent.Type == DType.Money)
                {
                    result.Fields.Add(new CompareNumberField(fieldCurrent, fieldPrev));
                }
            }
            return result;
        }
    }
}