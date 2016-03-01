using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Qa.Excel
{
    public static class ExcelExtension
    {
        public static Row Add(this Row row, string value)
        {
            var cell = new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(value)
            };
            row.AppendChild(cell);
            return row;
        }

        public static Row Add(this Row row, double value)
        {
            var cell = new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(value.ToString(CultureInfo.InvariantCulture))
            };
            row.AppendChild(cell);
            return row;
        }
    }
}