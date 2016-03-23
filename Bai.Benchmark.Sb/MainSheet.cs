using System;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Core;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Bai.Benchmark.Sb
{
    public class MainSheet : IExportPage
    {
        private const int INIT_COLUMN = 2;

        public void Print(ComparePacket packet, ExcelWorksheet sheet)
        {
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Structure.Name);
            
            cursor.Column(INIT_COLUMN).Row(5);
            print(packet, cursor);
            cursor.Down(2);
            
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }

        private void print(ComparePacket packet, ExcelCursor cursor)
        {
            var first = packet.Reports.First();
            var initRow = cursor.Pos.Row;
            var fieldsCount = first.Numbers.Count - 1;

            cursor
                .TopLeftBorderCorner()
                .Header("", formatDate(first.FileName))
                .Down()
                .Header("", "Values")
                .Down()
                .Print("Total Records", new TypedValue(first.RowsCount.Current, NumberFormat.Integer))
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
                    .Header(formatDate(report.FileName)).Merge(2)
                    .Down()
                    .Header("Values", "Change")
                    .Down()
                    .Integer(report.RowsCount.Current)
                    .Right()
                    .Percent(report.RowsCount.Change, StyleConditions.ChangePercent)
                    .Down()
                    .Left()
                    .PrintDown(report.Numbers.Select(x => x.GetCurrent()))
                    .Right()
                    .PrintDown(report.Numbers.Select(x => x.GetChange()), StyleConditions.ChangePercent)
                    .Down(fieldsCount)
                    .DrawBorder(ExcelBorderStyle.Thick)
                    .Right();
            }

            cursor.Sheet.View.FreezePanes(4, 3);

            if (packet.UniqueCounts.Any())
            {
                uniqueCounts(cursor, packet);
            }
            if (packet.UniqueFields.Any())
            {
                uniqueFields(cursor, packet);
            }
            if (packet.GroupedSums.Any())
            {
                groupedSums(cursor, packet);
            }
        }

        private static string formatDate(string fileName)
        {
            var parts = fileName.Split('.');
            var parsedDate = DateTime.Parse($"{parts[3].Substring(4, 2)}/{parts[3].Substring(6, 2)}/{parts[3].Substring(0,4)}");
            var monthName = DateExtention.ToMonthName(parsedDate.Month);
            return $"{monthName} {parsedDate.Year}";
        }

        private static void uniqueCounts(ExcelCursor cursor, ComparePacket packet)
        {
            if (!packet.UniqueCounts.Any())
            {
                return;
            }

            cursor
                .Down(3)
                .Column(INIT_COLUMN)
                .TopLeftBorderCorner()
                .Header("")
                .Right()
                .Header(packet.Reports.Select(x => formatDate(x.FileName)).First())
                .Down()
                .Left()
                .Header("", "Values")
                .Right();

            foreach (var report in packet.Reports.Skip(1))
            {
                cursor
                    .Up()
                    .Right()
                    .Header(formatDate(report.FileName))
                    .Merge(2)
                    .TopLeftBorderCorner()
                    .Down()
                    .Header("Values", "Change")
                    .Right();
            }

            foreach (var field in packet.UniqueCounts)
            {
                cursor
                    .Column(INIT_COLUMN)
                    .Down()
                    .Print(field.Title)
                    .Right()
                    .Integer(field.Counts.First().Current);

                foreach (var compareNumber in field.Counts.Skip(1))
                {
                    cursor
                        .Right()
                        .Integer(compareNumber.Current)
                        .Right()
                        .Percent(compareNumber.Change, StyleConditions.ChangePercent)
                        .DrawBorder(ExcelBorderStyle.Thick, field == packet.UniqueCounts.Last());
                }
            }

            cursor
                .DrawBorder(ExcelBorderStyle.Thick)
                .Down();
        }

        private static void uniqueFields(ExcelCursor cursor, ComparePacket packet)
        {
            foreach (var field in packet.UniqueFields)
            {
                cursor.Down(2)
                    .Column(INIT_COLUMN)
                    .Header(field.Title)
                    .TopLeftBorderCorner()
                    .MergeDown(2)
                    .Right()
                    .Header(packet.Reports.Select(x => formatDate(x.FileName)).First())
                    .Down()
                    .Header("Values")
                    .Left()
                    .Down();

                var startRow = cursor.Pos.Row;

                foreach (var key in field.Keys)
                {
                    cursor
                        .Print(key)
                        .Right()
                        .Print(field.ValueLists[0].GetCurrent(key))
                        .DrawBorder(ExcelBorderStyle.Thick, key == field.Keys.Last())
                        .Left()
                        .Down();
                }

                cursor
                    .Row(startRow)
                    .Column(INIT_COLUMN)
                    .Up()
                    .Right();

                foreach (var report in packet.Reports.Skip(1))
                {
                    cursor
                        .Up()
                        .Right()
                        .TopLeftBorderCorner()
                        .Header(formatDate(report.FileName))
                        .Merge(2)
                        .Down()
                        .Header("Values", "Change")
                        .Right();
                }

                cursor
                    .Row(startRow)
                    .Column(INIT_COLUMN)
                    .Right();

                foreach (var key in field.Keys)
                {
                    for (var i = 1; i < field.ValueLists.Count; i++)
                    {
                        cursor
                            .Right()
                            .Print(field.ValueLists[i].GetCurrent(key))
                            .Right()
                            .Print(field.ValueLists[i].GetChange(key), StyleConditions.ChangePercent)
                            .DrawBorder(ExcelBorderStyle.Thick, key == field.Keys.Last());
                    }

                    cursor
                        .Down()
                        .Left((packet.Reports.Count - 1) * 2);
                }
            }
        }

        private static void groupedSums(ExcelCursor cursor, ComparePacket packet)
        {
            foreach (var field in packet.GroupedSums)
            {
                cursor.Down(2)
                    .Column(INIT_COLUMN)
                    .Header(field.GroupByField.Title)
                    .TopLeftBorderCorner()
                    .MergeDown(2)
                    .Right()
                    .Header(packet.Reports.Select(x => formatDate(x.FileName)).First())
                    .Down()
                    .Header("Values")
                    .Left()
                    .Down();

                var startRow = cursor.Pos.Row;

                foreach (var key in field.Keys)
                {
                    cursor
                        .Print(key)
                        .Right()
                        .Print(field.ValueLists[0].GetCurrent(key))
                        .DrawBorder(ExcelBorderStyle.Thick, key == field.Keys.Last())
                        .Left()
                        .Down();
                }

                cursor
                    .Row(startRow - 2)
                    .Column(INIT_COLUMN + 2);

                foreach (var report in packet.Reports.Skip(1))
                {
                    cursor
                        .TopLeftBorderCorner()
                        .Header(formatDate(report.FileName))
                        .Merge(2)
                        .Down()
                        .Header("Values", "Change")
                        .Right(2)
                        .Up();
                }

                cursor
                    .Down(2)
                    .Column(INIT_COLUMN + 1);

                foreach (var key in field.Keys)
                {
                    foreach (var list in field.ValueLists.Skip(1))
                    {
                        cursor
                            .Right()
                            .Print(list.GetCurrent(key))
                            .Right()
                            .Print(list.GetChange(key), StyleConditions.ChangePercent)
                            .DrawBorder(ExcelBorderStyle.Thick, key == field.Keys.Last());
                    }

                    cursor
                        .Down()
                        .Left((packet.Reports.Count - 1) * 2);
                }
            }
        }
    }
}
