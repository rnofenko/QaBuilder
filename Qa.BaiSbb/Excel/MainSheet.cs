using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.BaiSbb.Compare;
using Qa.Core;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.BaiSbb.Excel
{
    public class MainSheet
    {
        public void Print(ComparePacket packet, ExcelWorksheet sheet)
        {
            const int initColumn = 2;
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Structure.Name);
            
            cursor.Column(initColumn).Row(5);
            printTotal(packet.Reports, cursor);
            cursor.Down(2);
            
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }

        private static void printTotal(IList<CompareReport> reports, ExcelCursor cursor)
        {
            var first = reports.First();
            var initRow = cursor.Pos.Row;
            var fieldsCount = first.Fields.Count - 1;

            cursor
                .TopLeftBorderCorner()
                .PrintAndCenter("", formatDate(first.FileName)).BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .PrintAndCenter("", "Values").BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .Print("Total Records", reports.First().RowsCount.Current)
                .Down()
                .PrintDown(first.Fields.Select(x => x.Title))
                .Right()
                .PrintDown(first.Fields.Select(x => new TypedValue(x.CurrentSum, x.Type)))
                .Down(fieldsCount)
                .DrawBorder(ExcelBorderStyle.Thick)
                .Right();

            foreach (var report in reports.Skip(1))
            {
                cursor.Row(initRow)
                    .TopLeftBorderCorner()
                    .Print(formatDate(report.FileName)).Merge(2).BackgroundColor(QaColor.HeaderBackground, 2)
                    .Down()
                    .PrintAndCenter("Values", "Change").BackgroundColor(QaColor.HeaderBackground, 2)
                    .Down()
                    .Print(report.RowsCount.Current,
                            new TypedValue(report.RowsCount.Current * 1.0 / report.RowsCount.Previous - 1, DType.Percent))
                    .Down()
                    .PrintDown(report.Fields.Select(x => new TypedValue(x.CurrentSum, x.Type)))
                    .Right()
                    .PrintDown(report.Fields.Select(x => new TypedValue(x.Change, DType.Percent)), StyleConditions.ChangePercent)
                    .Down(fieldsCount)
                    .DrawBorder(ExcelBorderStyle.Thick)
                    .Right();
            }
        }

        private static string formatDate(string fileName)
        {
            var parts = fileName.Split('.');
            var parsedDate = DateTime.Parse($"{parts[3].Substring(4, 2)}/{parts[3].Substring(6, 2)}/{parts[3].Substring(0,4)}");
            var monthName = DateExtention.ToMonthName(parsedDate.Month);
            return $"{monthName} {parsedDate.Year}";
        }
    }
}
