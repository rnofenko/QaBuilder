using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Bai.Pulse.Excel
{
    public class PulseExporter : IExporter
    {
        private readonly MainPage _page;

        public PulseExporter()
        {
            _page = new MainPage();
        }

        public void Export(List<ComparePacket> packets, Settings settings)
        {
            var fileName = settings.QaFileName ?? packets.First().Structure.Name;
            var path = Path.Combine(settings.WorkingFolder, string.Format("{0}.xlsx", fileName));
            new PoliteDeleter().Delete(path);

            var file = new FileInfo(path);
            using (var package = new ExcelPackage(file))
            {
                var sheet = package.Workbook.Worksheets.Add(packets.First().Structure.Name);

                _page.Print(packets.First(x => x.SplitValue == PulseConsts.NATIONAL), sheet);
                foreach (var packet in packets.Where(x => x.SplitValue != PulseConsts.NATIONAL))
                {
                    _page.PrintState(packet, sheet);
                }
                _page.Footer(sheet);

                package.Save();
            }
            Process.Start(path);
        }
    }
}