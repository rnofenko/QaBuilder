using System.Diagnostics;
using System.IO;
using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Excel
{
    public class Exporter: IExporter
    {
        private readonly IExportPage _page;
        private ExcelPackage _excelPackage;
        private string _path;

        public Exporter(IExportPage page)
        {
            _page = page;
        }

        public void AddData(string structureName, ComparePacket packet, Settings settings)
        {
            var package = getExcelPackage(structureName, settings);
            fillPacket(structureName, packet, package.Workbook);
        }

        public void Export()
        {
            _excelPackage.Save();
            Process.Start(_path);
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
        
        private void fillPacket(string structureName, ComparePacket packet, ExcelWorkbook book)
        {
            var sheet = book.Worksheets.Add(structureName);
            _page.Print(structureName, packet, sheet);
        }

        public void Dispose()
        {
            if (_excelPackage != null)
            {
                _excelPackage.Dispose();
            }
        }
    }
}