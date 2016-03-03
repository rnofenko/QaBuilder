using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Structure;
using Color = System.Drawing.Color;

namespace Qa.Core.Excel
{
    public class ExcelCursor
    {
        public ExcelWorksheet Sheet { get; }

        private Pos _pos;
        public Pos Pos => _pos;

        public ExcelCursor(ExcelWorksheet sheet)
        {
            Sheet = sheet;
            _pos = new Pos();
        }

        public ExcelCursor Row(int rowPosition)
        {
            _pos.Row = rowPosition;
            return this;
        }

        public ExcelRange Cell => getCell(_pos);

        public ExcelCursor Merge(int count)
        {
            var range = Sheet.Cells[Pos.Row, _pos.Column, Pos.Row, _pos.Column + count - 1];
            range.Merge = true;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            return this;
        }

        public ExcelCursor Print(params string[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                Sheet.Cells[Pos.Row, _pos.Column + i].Value = values[i];
            }
            return this;
        }
        
        public ExcelCursor PrintDown(params string[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                Sheet.Cells[Pos.Row + i, _pos.Column].Value = values[i];
            }
            return this;
        }

        public ExcelCursor PrintDown(IEnumerable<string> values)
        {
            return PrintDown(values.ToArray());
        }

        public ExcelCursor PrintDown(IEnumerable<double> values, DType type)
        {
            var pos = _pos.Clone();
            foreach (var value in values)
            {
                Print(value, type, _pos);
                _pos.Row++;
            }
            return this;
        }

        public ExcelCursor Print(double value, DType type)
        {
            return Print(value, type, _pos);
        }

        public ExcelCursor Print(double value, DType type, Pos pos)
        {
            if (type == DType.Money)
            {
                return Money(value, pos);
            }
            if (type == DType.Int)
            {
                return Integer(value, pos);
            }
            throw new InvalidOperationException($"Type {type} isn't supported.");
        }

        public ExcelCursor Money(double value)
        {
            return Money(value, _pos);
        }

        public ExcelCursor Money(double value, Pos pos)
        {
            var cell = getCell(Pos);
            cell.Value = value;
            cell.Style.Numberformat.Format = "$#,##0;$(#,##0)";
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
            return Integer(value, _pos);
        }

        public ExcelCursor Integer(double value, Pos pos)
        {
            var cell = getCell(pos);
            cell.Value = value;
            cell.Style.Numberformat.Format = "#,##0;-#,##0";
            return this;
        }

        public ExcelCursor NextColumn()
        {
            _pos.Column++;
            return this;
        }

        public ExcelCursor Column(int columnPosition)
        {
            _pos.Column = columnPosition;
            return this;
        }

        public int GetColumn()
        {
            return _pos.Column;
        }

        public ExcelCursor Down(int count = 1)
        {
            _pos.Row += count;
            return this;
        }

        public ExcelCursor PrevColumn()
        {
            _pos.Column--;
            return this;
        }

        private ExcelRange getCell(Pos pos)
        {
            return Sheet.Cells[pos.Row, pos.Column];
        }
    }
}
