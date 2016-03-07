using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Bai.Sbb.Compare;
using Qa.Core;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Bai.Sbb.Excel
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
            var initColumn = cursor.Pos.Column;
            var fieldsCount = first.Numbers.Count - 1;
            
            cursor
                .TopLeftBorderCorner()
                .PrintAndCenter("", formatDate(first.FileName)).BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .PrintAndCenter("", "Values").BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .Print("Total Records", reports.First().RowsCount.Current)
                .Down()
                .PrintDown(first.Numbers.Select(x => x.Title))
                .Right()
                .PrintDown(first.Numbers.Select(x => x.GetCurrent()))
                .Down(fieldsCount)
                .DrawBorder(ExcelBorderStyle.Thick)
                .Right();
                //.Down(2)
                //.PrintAndCenter("", first.Numbers.First().UniqueValues.First().Value).BackgroundColor(QaColor.HeaderBackground, 2)

            foreach (var report in reports.Skip(1))
            {
                cursor.Row(initRow)
                    .TopLeftBorderCorner()
                    .Print(formatDate(report.FileName)).Merge(2).BackgroundColor(QaColor.HeaderBackground, 2)
                    .Down()
                    .PrintAndCenter("Values", "Change").BackgroundColor(QaColor.HeaderBackground, 2)
                    .Down()
                    .Print(report.RowsCount.Current, new TypedValue(report.RowsCount.Change, DType.Percent))
                    .Down()
                    .PrintDown(report.Numbers.Select(x => x.GetCurrent()))
                    .Right()
                    .PrintDown(report.Numbers.Select(x => x.GetChange()), StyleConditions.ChangePercent)
                    .Down(fieldsCount)
                    .DrawBorder(ExcelBorderStyle.Thick)
                    .Right();
            }

            cursor.Down(3).Column(initColumn);
            foreach (var report in reports)
            {
                foreach (var field in report.UniqueFields)
                {
                    var unique = field.UniqueValues.First();
                    cursor
                        .Row(initRow + fieldsCount)
                        .Down()
                        .Print(field.Title).Merge(2).BackgroundColor(QaColor.HeaderBackground, 2)
                        .Down()
                        .PrintAndCenter("Values", "Change").BackgroundColor(QaColor.HeaderBackground, 2)
                        .Down()
                        .PrintAndCenter(unique.Value);
                }
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
