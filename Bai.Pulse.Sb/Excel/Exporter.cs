using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Qa.Bai.Pulse.Sb.Compare;
using Qa.Core.System;

namespace Qa.Bai.Pulse.Sb.Excel
{
    public class Exporter
    {
        public void Export(List<PulseComparePacket> packets, CompareSettings settings)
        {
            var path = Path.Combine(settings.WorkingFolder, string.Format("{0}.xlsx", packets.First().Strucure.Name));
            new PoliteDeleter().Delete(path);

            var file = new FileInfo(path);
            using (var package = new ExcelPackage(file))
            {
                foreach (var packet in packets)
                {
                    fillPacket(packet, package.Workbook);
                }
                package.Save();
            }
            Process.Start(path);
        }
        
        private void fillPacket(PulseComparePacket packet, ExcelWorkbook book)
        {
            var sheet = book.Worksheets.Add(packet.Strucure.Name);
            new MainSheet().Print(packet, sheet);

            foreach (var report in packet.Reports)
            {
                sheet = book.Worksheets.Add(report.SubReports.First().FileName);
                new PeriodSheet().Print(packet, report, sheet);
            }
        }
    }
}