using System.Drawing;
using OfficeOpenXml.Style;
using Qa.Core.Excel;
using Qa.Core.System;

namespace Qa.Novantas.SaleScape.Dr.Excel
{
    public class Header
    {
        public void Print(ExcelCursor cursor, string reportName)
        {
            var logoCell = cursor.Sheet.Drawings.AddPicture("Logo", CommonResources.Logo());
            logoCell.SetPosition(0, 5, 0, 0);
            logoCell.SetSize(160, 40);

            for (var rowNumber = 1; rowNumber < 4; rowNumber++)
            {
                var row = cursor.Sheet.Row(rowNumber);
                row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 254, 28, 25));
            }
            cursor.Sheet.Row(1).Height = 10;
            cursor.Sheet.Row(3).Height = 10;


            var cell = cursor.Row(2).Column(4).Cell;
            cursor.Sheet.Cells[2, 4, 2, 13].Merge = true;
            cell.Value = reportName;
            cell.Style.Font.Size = 24;
            cell.Style.Font.Color.SetColor(Color.White);
        }
    }
}
