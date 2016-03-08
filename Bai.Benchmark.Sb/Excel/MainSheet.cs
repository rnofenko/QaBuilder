using System;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Bai.Benchmark.Sb.Compare;
using Qa.Bai.Sbb.Excel;
using Qa.Core;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Bai.Benchmark.Sb.Excel
{
    public class MainSheet
    {
        public void Print(ComparePacket packet, ExcelWorksheet sheet)
        {
            const int initColumn = 2;
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Structure.Name);
            
            cursor.Column(initColumn).Row(5);
            print(packet, cursor);
            cursor.Down(2);
            
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }

        private void print(ComparePacket packet, ExcelCursor cursor)
        {
            var first = packet.Reports.First();
            var initRow = cursor.Pos.Row;
            var initColumn = cursor.Pos.Column;
            var fieldsCount = first.Numbers.Count - 1;
            
            cursor
                .TopLeftBorderCorner()
                .PrintAndCenter("", formatDate(first.FileName)).BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .PrintAndCenter("", "Values").BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .Print("Total Records", packet.Reports.First().RowsCount.Current)
                .Down()
                .PrintDown(first.Numbers.Select(x => x.Title))
                .Right()
                .PrintDown(first.Numbers.Select(x => x.GetCurrent()))
                .Down(fieldsCount)
                .DrawBorder(ExcelBorderStyle.Thick)
                .Right();

            foreach (var report in packet.Reports.Skip(1))
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

            foreach (var field in packet.UniqueCounts)
            {
                cursor.Down()
                    .Print(field.Name).BackgroundColor(QaColor.HeaderBackground)
                    .Right()
                    .Print(field.Numbers.Select(x => x.Current))
                    .Left();
            }

            foreach (var field in packet.UniqueFields)
            {
                var set = field.UniqueValueSet;
                cursor.Down(3).Column(initColumn)
                    .Print(field.Name).BackgroundColor(QaColor.HeaderBackground)
                    .Down();

                foreach (var key in set.Keys)
                {
                    cursor.Print(key)
                        .Right()
                        .Print(set.Lists.Select(x => x.GetCurrent(key)))
                        .Left()
                        .Down();
                }
            }
        }
        
        private string formatDate(string fileName)
        {
            var parts = fileName.Split('.');
            var parsedDate = DateTime.Parse($"{parts[3].Substring(4, 2)}/{parts[3].Substring(6, 2)}/{parts[3].Substring(0,4)}");
            var monthName = DateExtention.ToMonthName(parsedDate.Month);
            return $"{monthName} {parsedDate.Year}";
        }
    }
}
