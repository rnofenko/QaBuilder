﻿using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Core.Compare;
using Qa.Core.Excel;

namespace Qa.Argus.Cd
{
    public class MainSheet: IExportPage
    {
        private const int INIT_COLUMN = 2;

        public void PrintReport(ComparePacket packet, ExcelWorksheet sheet)
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
            var fieldsCount = first.Numbers.Count - 1 + packet.UniqueCounts.Count;

            cursor
                .TopLeftBorderCorner()
                .Header("", first.FileName)
                .Down()
                .Header("", "Values")
                .Down()
                .Print("Total Records", first.RowsCount.CurrentAsInteger)
                .Down()
                .PrintDown(first.Numbers.Select(x => x.Title), packet.UniqueCounts.Select(x => x.Title + " (Unique values)"))
                .Right()
                .PrintDown(first.Numbers.Select(x => x.GetCurrent()), packet.UniqueCounts.Select(x => x.GetCurrent(first)), StyleConditions.ChangePercent)
                .Down(fieldsCount)
                .DrawBorder(ExcelBorderStyle.Thick)
                .Right();

            foreach (var report in packet.Reports.Skip(1))
            {
                cursor.Row(initRow)
                    .TopLeftBorderCorner()
                    .Header(report.FileName).Merge(2)
                    .Down()
                    .Header("Values", "Change")
                    .Down()
                    .Print(report.RowsCount.CurrentAsInteger, report.RowsCount.ChangeAsPercent)
                    .Down()
                    .PrintDown(report.Numbers.Select(x => x.GetCurrent()), packet.UniqueCounts.Select(x => x.GetCurrent(report)))
                    .Right()
                    .PrintDown(report.Numbers.Select(x => x.GetChange()), packet.UniqueCounts.Select(x => x.GetChange(report)), StyleConditions.ChangePercent)
                    .Down(fieldsCount)
                    .DrawBorder(ExcelBorderStyle.Thick)
                    .Right();
            }

            uniqueValues(cursor, packet);
        }

        private void uniqueValues(ExcelCursor cursor, ComparePacket packet)
        {
            var first = packet.Reports.First();

            foreach (var field in packet.UniqueValues)
            {
                cursor.Down(2)
                    .Column(INIT_COLUMN)
                    .Header(field.Title)
                    .TopLeftBorderCorner()
                    .MergeDown(2)
                    .Right()
                    .HeaderDown(first.FileName, "Values")
                    .Right();

                foreach (var report in packet.Reports.Skip(1))
                {
                    cursor
                        .TopLeftBorderCorner()
                        .Header(report.FileName)
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
                        .Column(INIT_COLUMN)
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
    }
}
