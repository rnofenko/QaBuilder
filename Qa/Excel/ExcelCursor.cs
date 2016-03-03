using System;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Structure;
using Color = System.Drawing.Color;

namespace Qa.Excel
{
    public class ExcelCursor
    {
        public ExcelWorksheet Sheet { get; }
        private int _row;
        private int _column;

        public ExcelCursor(ExcelWorksheet sheet)
        {
            Sheet = sheet;
        }

        public int GetRow()
        {
            return _row;
        }

        public ExcelCursor Row(int rowPosition)
        {
            _row = rowPosition;
            return this;
        }

        public ExcelRange Cell => Sheet.Cells[_row, _column];

        public ExcelCursor Merge(int count)
        {
            var range = Sheet.Cells[_row, _column, _row, _column + count - 1];
            range.Merge = true;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            return this;
        }

        public ExcelCursor Value(params string[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                Sheet.Cells[_row, _column + i].Value = values[i];
            }
            return this;
        }

        public ExcelCursor Value(double value, DType type)
        {
            if (type == DType.Money)
            {
                return Money(value);
            }
            if (type == DType.Int)
            {
                return Integer(value);
            }
            throw new InvalidOperationException($"Type {type} isn't supported.");
        }

        public ExcelCursor Money(double value)
        {
            Cell.Value = value;
            Cell.Style.Numberformat.Format = "$#,##0;$(#,##0)";
            return this;
        }

        public ExcelCursor Percent(double value)
        {
            Cell.Value = value;
            Cell.Style.Numberformat.Format = "#,##0%;-#,##0%";
            return this;
        }

        public ExcelCursor SetAsDanger()
        {
            Cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            Cell.Style.Fill.BackgroundColor.SetColor(Color.LightCoral);
            return this;
        }

        public ExcelCursor SetAsWarning()
        {
            Cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            Cell.Style.Fill.BackgroundColor.SetColor(Color.Bisque);
            return this;
        }

        public ExcelCursor Value(double value)
        {
            return Integer(value);
        }

        public ExcelCursor Integer(double value)
        {
            Cell.Value = value;
            Cell.Style.Numberformat.Format = "#,##0;-#,##0";
            return this;
        }

        public ExcelCursor NextColumn()
        {
            _column++;
            return this;
        }

        public ExcelCursor Column(int columnPosition)
        {
            _column = columnPosition;
            return this;
        }

        public int GetColumn()
        {
            return _column;
        }

        public ExcelCursor Down(int count = 1)
        {
            _row += count;
            return this;
        }
    }
}
