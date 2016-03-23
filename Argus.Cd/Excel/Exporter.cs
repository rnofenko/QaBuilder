using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.System;

namespace Qa.Argus.Cd.Excel
{
    public class Exporter
    {
        public void Export(List<ComparePacket> packets, CompareSettings settings)
        {
            var path = Path.Combine(settings.WorkingFolder, $"{packets.First().Structure.Name}.xlsx");
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
        
        private static void fillPacket(ComparePacket packet, ExcelWorkbook book)
        {
            var sheet = book.Worksheets.Add(packet.Structure.Name);
            new MainSheet().Print(packet, sheet);
        }
    }
}