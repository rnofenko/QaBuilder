using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Qa.Bai.Sbp.Compare;
using Qa.Bai.Sbp.Excel;
using Qa.Core.System;
using Qa.Sbpm.Compare;

namespace Qa.Bai.Pulse.Sb.Excel
{
    public class Exporter
    {
        public void Export(List<ComparePacket> packets, CompareSettings settings)
        {
            var path = Path.Combine(settings.WorkingFolder, $"{packets.First().Strucure.Name}.xlsx");
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
        
        private void fillPacket(ComparePacket packet, ExcelWorkbook book)
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