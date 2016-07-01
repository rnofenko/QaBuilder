using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Qa.Core.Parsers.FileReaders
{
    public class ExcelFileReader: IFileReader
    {
        private readonly ExcelPackage _excel;
        private readonly ExcelWorksheet _sheet;
        private readonly int _width;

        public ExcelFileReader(string path)
        {
            var content = File.ReadAllBytes(path);
            _excel = new ExcelPackage(new MemoryStream(content));
            _sheet = _excel.Workbook.Worksheets.First();
            _width = _sheet.Dimension.End.Column;
        }

        public void Dispose()
        {
            if (_excel != null)
            {
                _excel.Dispose();
            }
        }

        public void Skip(int lines)
        {
            RowNumber += lines;
        }

        public string[] ParseNextRow()
        {
            var parts = new string[_width];
            RowNumber++;
            for (var i = 0; i < _width; i++)
            {
                parts[i] = _sheet.Cells[RowNumber, i + 1].Value.ToString();
            }
            return parts;
        }

        public string ReadNextRow()
        {
            throw new global::System.NotImplementedException();
        }

        public int GetFieldsCount()
        {
            return _width;
        }

        public string GetLastLine()
        {
            return "Row number "+ RowNumber;
        }

        public int RowNumber { get; private set; }
    }
}