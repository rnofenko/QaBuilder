using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Novantas.SaleScape.Dr.Excel
{
    public class Exporter : IExporter
    {
        public const string OutputFileName = "Novantas SaleScape";
        public void Export(List<ComparePacket> packets, Settings settings)
        {
            var path = Path.Combine(settings.WorkingFolder, $"{OutputFileName}.xlsx");
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