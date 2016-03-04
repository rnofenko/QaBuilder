using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public class ExcelCursor
    {
        public ExcelWorksheet Sheet { get; }

        private Pos _pos;
        private Pos? _topLeftCorner;
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

        public ExcelCursor TopLeftBorderCorner()
        {
            _topLeftCorner = _pos;
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

        public ExcelCursor Print(params TypedValue[] values)
        {
            var pos = _pos.Clone();
            foreach (var value in values)
            {
                Print(value, pos);
                pos.Row++;
            }
            return this;
        }

        public ExcelCursor PrintAndCenter(params string[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                var range = Sheet.Cells[Pos.Row, _pos.Column + i];
                range.Value = values[i];
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
                Print(value, type, pos);
                pos.Row++;
            }
            return this;
        }

        public ExcelCursor PrintDown(IEnumerable<TypedValue> values, Action<StyleConditionArgs> styleCondition = null)
        {
            var pos = _pos.Clone();
            foreach (var value in values)
            {
                Print(value, pos);
                if (styleCondition != null)
                {
                    styleCondition(new StyleConditionArgs {Pos = pos, Amount = value.Double(), Type = value.Type, Cursor = this});
                }
                pos.Row++;
            }
            return this;
        }

        public ExcelCursor Print(double value, DType type)
        {
            return Print(value, type, _pos);
        }

        public ExcelCursor Print(TypedValue value, Pos pos)
        {
            return Print(value.Value, value.Type, pos);
        }

        public ExcelCursor Print(object value, DType type, Pos pos)
        {
            if (type == DType.Money)
            {
                return Money((double)value, pos);
            }
            if (type == DType.Double)
            {
                return Double((double)value, pos);
            }
            if (type == DType.Int)
            {
                return Integer((int)value, pos);
            }
            if (type == DType.Percent)
            {
                return Percent((double)value, pos);
            }
            if (type == DType.String)
            {
                return String((string)value, pos);
            }
            throw new InvalidOperationException($"Type {type} isn't supported.");
        }

        public ExcelCursor Money(double value)
        {
            return Money(value, _pos);
        }

        public ExcelCursor Money(double value, Pos pos)
        {
            var cell = getCell(pos);
            cell.Value = value;
            cell.Style.Numberformat.Format = "$#,##0;$(#,##0)";
            return this;
        }

        public ExcelCursor String(string value, Pos pos)
        {
            var cell = getCell(pos);
            cell.Value = value;
            return this;
        }

        public ExcelCursor Double(double value, Pos pos)
        {
            var cell = getCell(pos);
            cell.Value = value;
            cell.Style.Numberformat.Format = "#,##0.0;-#,##0.0";
            return this;
        }

        public ExcelCursor Percent(double value)
        {
            return Percent(value, _pos);
        }

        public ExcelCursor Percent(double value, Pos pos)
        {
            var cell = getCell(pos);
            cell.Value = value;
            cell.Style.Numberformat.Format = "#,##0%;-#,##0%";
            return this;
        }

        public ExcelCursor SetAsDanger()
        {
            return SetAsDanger(_pos);
        }

        public ExcelCursor SetAsDanger(Pos pos)
        {
            var style = getCell(pos).Style;
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(QaColor.Danger);
            return this;
        }

        public ExcelCursor SetAsWarning()
        {
            return SetAsWarning(_pos);
        }

        public ExcelCursor SetAsWarning(Pos pos)
        {
            var style = getCell(pos).Style;
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(QaColor.Warning);
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

        public ExcelCursor Right(int count = 1)
        {
            _pos.Column += count;
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

        public ExcelCursor DrawBorder(ExcelBorderStyle style = ExcelBorderStyle.Thin)
        {
            var topLeft = _topLeftCorner ?? _pos;

            var pos = new Pos {Column = topLeft.Column};//left
            for (pos.Row = topLeft.Row; pos.Row <= _pos.Row; pos.Row++)
            {
                getCell(pos).Style.Border.Left.Style = style;
            }
            pos.Column = _pos.Column;//right
            for (pos.Row = topLeft.Row; pos.Row <= _pos.Row; pos.Row++)
            {
                getCell(pos).Style.Border.Right.Style = style;
            }
            pos.Row = topLeft.Row;//top
            for (pos.Column = topLeft.Column; pos.Column <= _pos.Column; pos.Column++)
            {
                getCell(pos).Style.Border.Top.Style = style;
            }
            pos.Row = _pos.Row;//bottom
            for (pos.Column = topLeft.Column; pos.Column <= _pos.Column; pos.Column++)
            {
                getCell(pos).Style.Border.Bottom.Style = style;
            }
            _topLeftCorner = null;
            return this;
        }

        public ExcelCursor Center()
        {
            Cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            return this;
        }

        public ExcelCursor BackgroundColor(Color color, int cellsCount = 1)
        {
            var style = Sheet.Cells[Pos.Row, _pos.Column, Pos.Row, _pos.Column + cellsCount - 1].Style;
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(color);
            return this;
        }
    }
}