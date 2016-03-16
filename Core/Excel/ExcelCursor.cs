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
        private readonly Queue<Pos> _topLeftCorners;
        private readonly HeaderStyle _headerStyle;
        public Pos Pos => _pos;

        public ExcelCursor(ExcelWorksheet sheet)
        {
            Sheet = sheet;
            _headerStyle = new HeaderStyle
            {
                BackgroundColor = QaColor.HeaderBackground,
                HorizontalAlignment = ExcelHorizontalAlignment.Center
            };
            _pos = new Pos();
            _topLeftCorners = new Queue<Pos>();
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

        public ExcelCursor MergeDown(int count)
        {
            var range = Sheet.Cells[Pos.Row, _pos.Column, Pos.Row + count - 1, _pos.Column];
            range.Merge = true;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            return this;
        }

        public ExcelCursor TopLeftBorderCorner()
        {
            _topLeftCorners.Enqueue(_pos);
            return this;
        }

        public ExcelCursor Print(params string[] values)
        {
            var pos = _pos.Clone();
            foreach (var value in values)
            {
                String(value, pos);
                pos.Column++;
            }
            return this;
        }

        public ExcelCursor Print(IEnumerable<string> values)
        {
            return Print(values.ToArray());
        }

        public ExcelCursor Print(IEnumerable<double> values)
        {
            var pos = _pos.Clone();
            foreach (var value in values)
            {
                Double(value, pos);
                pos.Column++;
            }
            return this;
        }

        public ExcelCursor Print(params TypedValue[] values)
        {
            var pos = _pos.Clone();
            foreach (var value in values)
            {
                Print(value, pos);
                pos.Column++;
            }
            return this;
        }

        public ExcelCursor Header(params string[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                var range = Sheet.Cells[Pos.Row, _pos.Column + i];
                range.Value = values[i];
                range.Style.HorizontalAlignment = _headerStyle.HorizontalAlignment;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(_headerStyle.BackgroundColor);
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
                Print(new TypedValue(value, type), pos);
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
                    styleCondition(new StyleConditionArgs {Pos = pos, Value = value, Cursor = this});
                }
                pos.Row++;
            }
            return this;
        }

        public ExcelCursor Print(double value, DType type)
        {
            return Print(new TypedValue(value, type), _pos);
        }

        public ExcelCursor Print(TypedValue value, Pos pos)
        {
            if (value.Type == DType.Money)
            {
                return Money(value.Double(), pos);
            }
            if (value.Type == DType.Double)
            {
                return Double(value.Double(), pos);
            }
            if (value.Type == DType.Int)
            {
                return Integer(value.Int(), pos);
            }
            if (value.Type == DType.Percent)
            {
                return Percent(value.NullableDouble(), pos);
            }
            if (value.Type == DType.String)
            {
                return String(value.String(), pos);
            }
            throw new InvalidOperationException($"Type {value.Type} isn't supported.");
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

        public ExcelCursor Percent(double? value, Action<StyleConditionArgs> styleCondition = null)
        {
            return Percent(value, _pos, styleCondition);
        }

        public ExcelCursor Percent(double? value, Pos pos, Action<StyleConditionArgs> styleCondition = null)
        {
            var cell = getCell(pos);
            if (value == null)
            {
                cell.Value = "NA";
            }
            else
            {
                cell.Value = value;
                cell.Style.Numberformat.Format = "#,##0%;-#,##0%";
            }
            if (styleCondition != null)
            {
                styleCondition(new StyleConditionArgs { Pos = pos, Value = new TypedValue(value, DType.Percent), Cursor = this });
            }
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

        public ExcelCursor Left(int count = 1)
        {
            _pos.Column -= count;
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

        public ExcelCursor Up(int count = 1)
        {
            _pos.Row -= count;
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
            var topLeft = _pos;
            if (_topLeftCorners.Any())
            {
                topLeft = _topLeftCorners.Dequeue();
            }
            
            return DrawBorder(topLeft, style);
        }

        public ExcelCursor DrawBorder(Pos topLeft, ExcelBorderStyle style = ExcelBorderStyle.Thin)
        {
            return DrawBorder(topLeft, _pos, style);
        }

        public ExcelCursor DrawBorder(Pos topLeft, Pos bottomRight, ExcelBorderStyle style = ExcelBorderStyle.Thin)
        {
            var pos = new Pos { Column = topLeft.Column };//left
            for (pos.Row = topLeft.Row; pos.Row <= bottomRight.Row; pos.Row++)
            {
                getCell(pos).Style.Border.Left.Style = style;
            }
            pos.Column = bottomRight.Column;//right
            for (pos.Row = topLeft.Row; pos.Row <= bottomRight.Row; pos.Row++)
            {
                getCell(pos).Style.Border.Right.Style = style;
            }
            pos.Row = topLeft.Row;//top
            for (pos.Column = topLeft.Column; pos.Column <= bottomRight.Column; pos.Column++)
            {
                getCell(pos).Style.Border.Top.Style = style;
            }
            pos.Row = bottomRight.Row;//bottom
            for (pos.Column = topLeft.Column; pos.Column <= bottomRight.Column; pos.Column++)
            {
                getCell(pos).Style.Border.Bottom.Style = style;
            }
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