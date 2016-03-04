using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Qa.BaiSbb.Compare;

namespace Qa.BaiSbb.Excel
{
    public class Exporter
    {
        public void Export(List<ComparePacket> packets, CompareSettings settings)
        {
            var path = Path.Combine(settings.WorkingFolder, $"{packets.First().Structure.Name}.xlsx");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var file = new FileInfo(path);
            using (var package = new ExcelPackage(file))
            {
                foreach (var packet in packets)
                {
                    FillPacket(packet, package.Workbook);
                }
                package.Save();
            }
            Process.Start(path);
        }
        
        private static void FillPacket(ComparePacket packet, ExcelWorkbook book)
        {
            var sheet = book.Worksheets.Add(packet.Structure.Name);
            new MainSheet().Print(packet, sheet);
        }
    }
}