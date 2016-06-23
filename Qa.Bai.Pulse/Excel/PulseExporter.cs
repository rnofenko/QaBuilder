using System.Diagnostics;
using System.IO;
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
        private ExcelPackage _excelPackage;
        private string _path;
        private ExcelWorksheet _sheet;

        public PulseExporter()
        {
            _page = new MainPage();
        }

        public void AddData(string structureName, ComparePacket packet, Settings settings)
        {
            var package = getExcelPackage(structureName, settings);
            if (_sheet == null)
            {
                _sheet = package.Workbook.Worksheets.Add(structureName);
                _page.Print(structureName, packet, _sheet);
            }
            else
            {
                _page.PrintState(packet, _sheet);
            }
        }
        
        private ExcelPackage getExcelPackage(string structureName, Settings settings)
        {
            if (_excelPackage == null)
            {
                var fileName = settings.QaFileName ?? structureName;
                _path = Path.Combine(settings.WorkingFolder, string.Format("{0}.xlsx", fileName));
                new PoliteDeleter().Delete(_path);

                var file = new FileInfo(_path);
                _excelPackage = new ExcelPackage(file);
            }
            return _excelPackage;
        }

        public void Dispose()
        {
            if (_excelPackage != null)
            {
                _excelPackage.Dispose();
            }
        }

        public void Export()
        {
            _page.Footer(_sheet);
            _excelPackage.Save();
            Process.Start(_path);
        }
    }
}