using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Core.Excel;
using Qa.Core.Structure;
using Qa.Sbpm.Compare;

namespace Qa.Sbpm.Excel
{
    public class MainSheet
    {
        public void Print(ComparePacket packet, ExcelWorksheet sheet)
        {
            const int initColumn = 2;
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Strucure.Name);
            
            cursor.Column(initColumn).Row(5);
            printTotal(packet.Reports.Select(x => x.GetSubReport(QaSettings.National)).ToList(), cursor);
            cursor.Down(2);

            foreach (var state in packet.States.Where(x => x != QaSettings.National))
            {
                cursor.Column(initColumn);
                printStates(packet.GetTransformedSubReports(QaSettings.TransformByState, state), cursor);
                cursor.Down(2);
            }
            
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }

        private void printTotal(IList<CompareSubReport> reports, ExcelCursor cursor)
        {
            var first = reports.First();
            var initRow = cursor.Pos.Row;
            var fieldsCount = first.Fields.Count;

            cursor
                .TopLeftBorderCorner()
                .Print("", first.FileName).BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .Print("", "Values").BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .PrintDown(first.Fields.Select(x => x.Title))
                .Right()
                .PrintDown(first.Fields.Select(x => new TypedAmount {Amount = x.CurrentSum, Type = x.Type}))
                .Down(fieldsCount)
                .DrawBorder(ExcelBorderStyle.Thick)
                .Right();

            foreach (var report in reports.Skip(1))
            {
                cursor.Row(initRow)
                    .TopLeftBorderCorner()
                    .Print(report.FileName).Merge(2).BackgroundColor(QaColor.HeaderBackground, 2)
                    .Down()
                    .Print("Values", "Change").BackgroundColor(QaColor.HeaderBackground, 2)
                    .Down()
                    .PrintDown(report.Fields.Select(x => new TypedAmount {Amount = x.CurrentSum, Type = x.Type}))
                    .Right()
                    .PrintDown(report.Fields.Select(x => new TypedAmount {Amount = x.Change, Type = DType.Percent}), StyleConditions.ChangePercent)
                    .Down(fieldsCount)
                    .DrawBorder(ExcelBorderStyle.Thick)
                    .Right();
            }
        }

        private void printStates(IList<CompareSubReport> reports, ExcelCursor cursor)
        {
            var first = reports.First();
            cursor
                .Print(first.State)
                .BackgroundColor(QaColor.HeaderBackground)
                .Down();
            var fieldsCount = first.Fields.Count - 1;
            var initRow = cursor.Pos.Row;

            cursor
                .TopLeftBorderCorner()
                .Print("", first.FileName).BackgroundColor(QaColor.HeaderBackground, 2)
                .Down()
                .PrintDown(first.Fields.Select(x => x.Title))
                .Right()
                .PrintDown(first.Fields.Select(x => new TypedAmount {Amount = x.CurrentSum, Type = x.Type}))
                .Down(fieldsCount)
                .DrawBorder(ExcelBorderStyle.Thick)
                .Right();

            foreach (var report in reports.Skip(1))
            {
                cursor
                    .Row(initRow)
                    .TopLeftBorderCorner()
                    .Print(report.FileName).Merge(2).BackgroundColor(QaColor.HeaderBackground)
                    .Down()
                    .PrintDown(report.Fields.Select(x => new TypedAmount {Amount = x.CurrentSum, Type = x.Type}))
                    .Right()
                    .PrintDown(report.Fields.Select(x => new TypedAmount {Amount = x.Change, Type = DType.Percent}), StyleConditions.ChangePercent)
                    .Down(fieldsCount)
                    .DrawBorder(ExcelBorderStyle.Thick)
                    .Right();
            }
        }
    }
}
