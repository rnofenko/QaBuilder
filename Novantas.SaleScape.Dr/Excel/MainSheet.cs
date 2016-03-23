using System;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Novantas.SaleScape.Dr.Compare;
using Qa.Core;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Novantas.SaleScape.Dr.Excel
{
    public class MainSheet
    {
        private const int INIT_COLUMN = 2;

        public void Print(ComparePacket packet, ExcelWorksheet sheet)
        {
            
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, $"{packet.Structure.Name}");

            cursor.Column(INIT_COLUMN).Row(5);
            print(packet, cursor);
            cursor.Down(2);
            
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }

        private static void print(ComparePacket packet, ExcelCursor cursor)
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
                    .PrintDown(report.Numbers.Select(x => x.GetCurrent().Double()), NumberFormat.Money)
                    .Right()
                    .PrintDown(report.Numbers.Select(x => x.GetChange()), StyleConditions.ChangePercent)
                    .Down(fieldsCount)
                    .DrawBorder(ExcelBorderStyle.Thick)
                    .Right();
            }

            cursor.Sheet.View.FreezePanes(4, 3);

            uniqueCounts(cursor, packet);

            uniqueFields(cursor, packet);

            groupedSums(cursor, packet);

        }

        private static string formatDate(string fileName)
        {
            var parts = fileName.Split('_');

            var parsedDate = DateTime.Parse(parts.Length == 2 
                ? 
                $"{parts[1].Substring(4, 2)}/01/{parts[1].Substring(0, 4)}" 
                : 
                $"{parts[2].Substring(4, 2)}/01/{parts[2].Substring(0, 4)}");

            var monthName = DateExtention.ToMonthName(parsedDate.Month);

            return $"{monthName} {parsedDate.Year}";
        }

        private static void uniqueCounts(ExcelCursor cursor, ComparePacket packet)
        {
            if (packet.UniqueCounts.Any())
            {
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
                        .Integer(field.UniqueValueCounts.First().Current);

                    foreach (var compareNumber in field.SumNumbers.Skip(1))
                    {
                        cursor
                            .Right()
                            .Integer(compareNumber.Current)
                            .Right()
                            .Percent(compareNumber.Change, StyleConditions.ChangePercent);

                        if (field == packet.UniqueCounts.Last())
                        {
                            cursor.DrawBorder(ExcelBorderStyle.Thick);
                        }
                    }
                }

                cursor
                    .DrawBorder(ExcelBorderStyle.Thick)
                    .Down();
            }
        }

        private static void uniqueFields(ExcelCursor cursor, ComparePacket packet)
        {
            foreach (var field in packet.UniqueFields)
            {
                var set = field.UniqueValues;
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

                foreach (var key in set.Keys)
                {
                    cursor
                        .Print(key)
                        .Right()
                        .Integer(set.Lists[0].GetCurrent(key));


                    if (key == set.Keys.Last())
                    {
                        cursor
                            .DrawBorder(ExcelBorderStyle.Thick);
                    }

                    cursor.Left()
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

                foreach (var key in set.Keys)
                {
                    for (var i = 1; i < set.Lists.Count; i++)
                    {
                        cursor
                            .Right()
                            .Integer(set.Lists[i].GetCurrent(key))
                            .Right()
                            .Percent(set.Lists[i].GetChange(key), StyleConditions.ChangePercent);

                        if (key == set.Keys.Last())
                        {
                            cursor
                                .DrawBorder(ExcelBorderStyle.Thick);
                        }
                    }

                    cursor
                        .Down()
                        .Left((packet.Reports.Count - 1)*2);
                }
            }
        }

        private static void groupedSums(ExcelCursor cursor, ComparePacket packet)
        {
            foreach (var field in packet.GroupedSums)
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
                        .Integer(field.ValueLists[0].GetCurrent(key))
                        .DrawBorder(ExcelBorderStyle.Thick, key == field.Keys.Last())
                        .Left()
                        .Down();
                }

                cursor
                    .Row(startRow)
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
                            .Integer(list.GetCurrent(key))
                            .Right()
                            .Percent(list.GetChange(key), StyleConditions.ChangePercent)
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
