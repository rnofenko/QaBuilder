using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Core.System;
using Qa.Excel;
using Qa.Sbpm.Compare;

namespace Qa.Sbpw.Excel
{
    public class EpExporterToExcel
    {
        public void Export(List<ComparePacket> packets, CompareSettings settings)
        {
            var path = Path.Combine(settings.WorkingFolder, "comparing.xlsx");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var file = new FileInfo(path);
            using (var package = new ExcelPackage(file))
            {
                foreach (var packet in packets)
                {
                    fillPacket(packet, package.Workbook);
                }
                package.Save();
            }
            Process.Start(path);
        }

        private void putHeader(ExcelCursor cursor, string reportName)
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
            cursor.Sheet.Cells[2, 4, 2, 8].Merge = true;
            cell.Value = reportName;
            cell.Style.Font.Size = 24;
            cell.Style.Font.Color.SetColor(Color.White);
        }

        private void fillPacket(ComparePacket packet, ExcelWorkbook book)
        {
            var sheet = book.Worksheets.Add(packet.Strucure.Name);
            printMainSheet(packet, sheet);

            foreach (var report in packet.Reports)
            {
                sheet = book.Worksheets.Add(report.Summary.FileName);
                printForOnePeriod(packet, report, sheet);
            }
        }

        private void printForOnePeriod(ComparePacket packet, CompareReport report, ExcelWorksheet sheet)
        {
            const int initColumn = 2;
            var cursor = new ExcelCursor(sheet);
            putHeader(cursor, packet.Strucure.Name);

            cursor.Column(initColumn).Row(5);
            printSubReport(report.Summary, cursor);
            cursor.Down(4);

            foreach (var key in packet.AllKeys)
            {
                cursor.Column(initColumn);
                printSubReport(report.GetSubReport(key), cursor);
                cursor.Down(4);
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
        }

        private void printMainSheet(ComparePacket packet, ExcelWorksheet sheet)
        {
            const int initColumn = 2;
            var cursor = new ExcelCursor(sheet);
            putHeader(cursor, packet.Strucure.Name);

            cursor.Column(initColumn).Row(5);
            printSubReportsForMainSheet(packet.Reports.Select(x => x.Summary).ToList(), cursor);
            cursor.Down(4);

            foreach (var key in packet.AllKeys)
            {
                cursor.Column(initColumn);
                printSubReportsForMainSheet(packet.Reports.Select(x => x.GetSubReport(key)).ToList(), cursor);
                cursor.Down(4);
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
        }

        private void printSubReport(CompareSubReport report, ExcelCursor cursor)
        {
            cursor.Value(report.Key ?? "TOTAL").Down();

            var initRow = cursor.GetRow();

            cursor.Row(initRow);
            cursor.Value(report.FileName).Merge(2).Down();
            cursor.Value("Field", "Values", "Change, %");
            cursor.Down();

            var startRow = cursor.GetRow();
            cursor.Row(startRow).Value(report.RowsCount.Current);
            foreach (var field in report.Fields)
            {
                cursor.Down().Value(field.CurrentSum, field.Type);
            }
            cursor.NextColumn();

            cursor.Row(startRow).Percent(report.RowsCount.Increase);
            foreach (var field in report.Fields)
            {
                cursor.Down().Percent(field.Change);
                if (Math.Abs(field.Change) > 0.35)
                {
                    cursor.SetAsDanger();
                }
                else if (Math.Abs(field.Change) > 0.20)
                {
                    cursor.SetAsWarning();
                }
            }

            cursor.Sheet.Cells[cursor.Sheet.Dimension.Address].AutoFitColumns();
        }

        private void printSubReportsForMainSheet(IList<CompareSubReport> reports, ExcelCursor cursor)
        {
            var first = reports.First();
            cursor.Value(first.Key ?? "TOTAL").Down();

            var initRow = cursor.GetRow();
            var isFirst = true;
            foreach (var file in reports)
            {
                cursor.Row(initRow);
                
                if (isFirst)
                {
                    cursor.Value("", file.FileName)
                        .Down()
                        .Value("Field", "Values");
                }
                else
                {
                    cursor.Value(file.FileName).Merge(2)
                        .Down()
                        .Value("Values", "Change, %");
                }
                cursor.Down();

                var startRow = cursor.GetRow();
                if (isFirst)
                {
                    cursor.Value("Rows");
                    foreach (var field in file.Fields)
                    {
                        cursor.Down().Value(field.Name);
                    }
                    cursor.NextColumn();
                }

                cursor.Row(startRow).Value(file.RowsCount.Current);
                foreach (var field in file.Fields)
                {
                    cursor.Down().Value(field.CurrentSum, field.Type);
                }
                cursor.NextColumn();

                if (!isFirst)
                {
                    cursor.Row(startRow).Percent(file.RowsCount.Increase);
                    foreach (var field in file.Fields)
                    {
                        cursor.Down().Percent(field.Change);
                        if (Math.Abs(field.Change) > 0.35)
                        {
                            cursor.SetAsDanger();
                        }
                        else if (Math.Abs(field.Change) > 0.20)
                        {
                            cursor.SetAsWarning();
                        }
                    }
                    cursor.NextColumn();
                }

                if (isFirst)
                {
                    isFirst = false;
                }

                cursor.NextColumn();
            }

            cursor.Sheet.Cells[cursor.Sheet.Dimension.Address].AutoFitColumns();
        }
    }
}