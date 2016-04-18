using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Bai.Pulse.Sb.Collectors;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Bai.Pulse.Sb.Compare
{
    public class PulseComparer
    {
        public List<PulseComparePacket> Compare(List<PulseRawReport> statistics)
        {
            var packets = statistics
                .GroupBy(x => x.Structure.Name)
                .Select(compare)
                .ToList();
            return packets;
        }

        private PulseComparePacket compare(IEnumerable<PulseRawReport> rawReports)
        {
            var reports = rawReports.OrderBy(x => x.Path).ToList();
            var first = reports.First();
            var packet = new PulseComparePacket {Strucure = first.Structure, States = reports.SelectMany(x => x.SubReports.Keys).Distinct().ToList()};
            
            PulseRawReport previous = null;
            foreach (var report in reports)
            {
                packet.Reports.Add(compare(report, previous, packet.States));
                previous = report;
            }

            return packet;
        }

        private PulseCompareReport compare(PulseRawReport current, PulseRawReport previous, List<string> states)
        {
            var fileName = Path.GetFileNameWithoutExtension(current.Path);
            var result = new PulseCompareReport();

            foreach (var key in states)
            {
                var currentSub = current.GetSubReport(key);
                RawSubReport previousSub = null;
                if (previous != null)
                {
                    previousSub = previous.GetSubReport(key);
                }
                var subResult = compare(currentSub, previousSub, key, fileName);
                result.SubReports.Add(subResult);
            }

            foreach (var transformation in current.TransformedReports.Keys)
            {
                PulseRawReport prevReport = null;
                if (previous != null)
                {
                    prevReport = previous.TransformedReports[transformation];
                }
                var report = compare(current.TransformedReports[transformation], prevReport, states);
                result.TransformReports.Add(transformation, report);
            }
            
            return result;
        }

        private CompareSubReport compare(RawSubReport current, RawSubReport previous, string key, string fileName)
        {
            var prevRowsCount = 0;
            if (previous != null)
            {
                prevRowsCount = previous.RowsCount;
            }
            var result = new CompareSubReport
            {
                RowsCount = new CompareNumber(current.RowsCount, prevRowsCount),
                State = key,
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

                if (fieldCurrent.Type == DType.Numeric)
                {
                    result.Fields.Add(new CompareField(fieldCurrent, fieldPrev));
                }
            }
            return result;
        }
    }
}