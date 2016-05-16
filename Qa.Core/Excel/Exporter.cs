using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Excel
{
    public class Exporter: IExporter
    {
        private readonly IExportPage _page;

        public Exporter(IExportPage page)
        {
            _page = page;
        }

        public void Export(List<ComparePacket> packets, Settings settings)
        {
            var fileName = settings.QaFileName ?? packets.First().Structure.Name;
            var path = Path.Combine(settings.WorkingFolder, string.Format("{0}.xlsx", fileName));
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
            var sheet = book.Worksheets.Add(packet.Structure.Name);
            _page.Print(packet, sheet);
        }
    }
}