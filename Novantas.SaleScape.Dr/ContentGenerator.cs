using System;
using System.Linq;
using System.Text.RegularExpressions;
using OfficeOpenXml.Style;
using Qa.Core;
using Qa.Core.Compare;
using Qa.Core.Excel;

namespace Qa.Novantas.SaleScape.Dr
{
    public class ContentGenerator
    {
        public void Print(ComparePacket packet, ExcelCursor cursor, int initColumn)
        {
            var startColumn = initColumn;
            var first = packet.Reports.First();
            var initRow = cursor.Pos.Row;
            var fieldsCount = first.Numbers.Count - 1;

            cursor
                .TopLeftBorderCorner()
                .Header("", formatDate(first.FileName))
                .Down()
                .Header("", "Values")
                .Down()
                .Print("Total Records", first.RowsCount.CurrentAsInteger)
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

            uniqueCounts(cursor, packet, startColumn);

            uniqueFields(cursor, packet, startColumn);

            groupedSums(cursor, packet, startColumn);
        }

        private void uniqueCounts(ExcelCursor cursor, ComparePacket packet, int startColumn)
        {
            if (!packet.UniqueCounts.Any())
            {
                return;
            }

            cursor
                .Down(3)
                .Column(startColumn)
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
                    .Column(MainSheet.InitColumn)
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

        private void uniqueFields(ExcelCursor cursor, ComparePacket packet, int startColumn)
        {
            foreach (var field in packet.UniqueFields)
            {
                cursor.Down(2)
                    .Column(startColumn)
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
                    .Column(startColumn)
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
                    .Column(startColumn)
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
                        .Left((packet.Reports.Count - 1)*2);
                }
            }
        }

        private void groupedSums(ExcelCursor cursor, ComparePacket packet, int startColumn)
        {
            foreach (var field in packet.GroupedSums)
            {
                cursor.Down(2)
                    .Column(startColumn)
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
                    .Column(startColumn + 2);

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
                    .Column(startColumn + 1);

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
                        .Left((packet.Reports.Count - 1)*2);
                }
            }
        }

        private string formatDate(string fileName)
        {
            var rgx = new Regex(@"\d{6,}");
            var mat = rgx.Match(fileName).ToString();

            var parsedDate = DateTime.Parse(mat.Length < 8
                ? $"{mat.Substring(4, 2)}/01/{mat.Substring(0, 4)}"
                : $"{mat.Substring(4, 2)}/{mat.Substring(6, 2)}/{mat.Substring(0, 4)}");

            var monthName = DateExtention.ToMonthName(parsedDate.Month);

            return $"{monthName} {parsedDate.Year}";
        }
    }
}