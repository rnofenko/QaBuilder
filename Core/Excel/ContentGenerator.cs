using System;
using System.Linq;
using System.Text.RegularExpressions;
using OfficeOpenXml.Style;
using Qa.Core.Compare;

namespace Qa.Core.Excel
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
                .Header("", FormatDate(first.FileName))
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
                    .Header(FormatDate(report.FileName)).Merge(2)
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
                .Header(packet.Reports.Select(x => FormatDate(x.FileName)).First())
                .Down()
                .Left()
                .Header("", "Values")
                .Right();

            foreach (var report in packet.Reports.Skip(1))
            {
                cursor
                    .Up()
                    .Right()
                    .Header(FormatDate(report.FileName))
                    .Merge(2)
                    .TopLeftBorderCorner()
                    .Down()
                    .Header("Values", "Change")
                    .Right();
            }

            foreach (var field in packet.UniqueCounts)
            {
                cursor
                    .Column(startColumn)
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
            var first = packet.Reports.First();

            foreach (var field in packet.UniqueValues)
            {
                cursor.Down(2)
                    .Column(startColumn)
                    .Header(field.Title)
                    .TopLeftBorderCorner()
                    .MergeDown(2)
                    .Right()
                    .HeaderDown(FormatDate(first.FileName), "Values")
                    .Right();

                foreach (var report in packet.Reports.Skip(1))
                {
                    cursor
                        .TopLeftBorderCorner()
                        .Header(FormatDate(report.FileName))
                        .Merge(2)
                        .Down()
                        .Header("Values", "Change")
                        .Right(2)
                        .Up();
                }

                cursor.Down();

                foreach (var key in field.Keys)
                {
                    cursor.Down()
                        .Column(startColumn)
                        .Print(field.GetTranslate(key))
                        .Right();

                    foreach (var file in packet.Reports)
                    {
                        cursor
                            .Print(field.GetCurrent(file, key))
                            .RightIf(file != first)
                            .PrintIf(file != first, field.GetChange(file, key), StyleConditions.ChangePercent)
                            .DrawBorder(ExcelBorderStyle.Thick, key == field.Keys.Last())
                            .Right();
                    }
                }
            }
        }

        private void groupedSums(ExcelCursor cursor, ComparePacket packet, int startColumn)
        {
            foreach (var field in packet.GroupedSums)
            {
                cursor.Down(2)
                    .Column(startColumn)
                    .Header(field.Description.Title)
                    .TopLeftBorderCorner()
                    .MergeDown(2)
                    .Right()
                    .Header(packet.Reports.Select(x => FormatDate(x.FileName)).First())
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
                        .Header(FormatDate(report.FileName))
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
                        .Left((packet.Reports.Count - 1) * 2);
                }
            }
        }

        public string FormatDate(string fileName)
        {
            var rgx = new Regex(@"\d{6,}");
            var match = rgx.Match(fileName).ToString();

            var parsedDate = DateTime.Parse(match.Length < 8
                ? $"{match.Substring(4, 2)}/01/{match.Substring(0, 4)}"
                : $"{match.Substring(4, 2)}/{match.Substring(6, 2)}/{match.Substring(0, 4)}");

            var monthName = DateExtention.ToMonthName(parsedDate.Month);

            return $"{monthName} {parsedDate.Year}";
        }
    }
}