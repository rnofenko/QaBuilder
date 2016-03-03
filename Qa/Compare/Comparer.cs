using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Collectors;
using Qa.Structure;

namespace Qa.Compare
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
            var packet = new ComparePacket {Strucure = first.Structure, AllKeys = reports.SelectMany(x => x.SubReports.Keys).Distinct().ToList()};
            
            RawReport previous = null;
            foreach (var report in reports)
            {
                packet.Reports.Add(compare(report, previous, packet.AllKeys));
                previous = report;
            }

            return packet;
        }

        private CompareReport compare(RawReport current, RawReport previous, List<string> allKeys)
        {
            var fileName = Path.GetFileNameWithoutExtension(current.Path);
            var result = new CompareReport
            {
                Summary = compare(current.Summary, previous?.Summary, null, fileName)
            };

            foreach (var key in allKeys)
            {
                var currentSub = current.GetSubReport(key);
                var previousSub = previous?.GetSubReport(key);
                var subResult = compare(currentSub, previousSub, key, fileName);
                result.SubReports.Add(subResult);
            }
            
            return result;
        }

        private CompareSubReport compare(RawSubReport current, RawSubReport previous, string key, string fileName)
        {
            var result = new CompareSubReport
            {
                RowsCount = new CompareNumber(current.RowsCount, previous?.RowsCount),
                Key = key,
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

                if (fieldCurrent.Type == DType.Float || fieldCurrent.Type == DType.Int || fieldCurrent.Type == DType.Money)
                {
                    result.Fields.Add(new CompareNumberField(fieldCurrent, fieldPrev));
                }
            }
            return result;
        }
    }
}