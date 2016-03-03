using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
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

            cursor
                .TopLeftBorderCorner()
                .Print("", first.FileName)
                .Down()
                .Print("Field", "Values")
                .Down()
                .PrintDown(first.Fields.Select(x => x.Title))
                .Right()
                .PrintDown(first.Fields.Select(x => new TypedAmount {Amount = x.CurrentSum, Type = x.Type}))
                //.DrawBorder()
                .Right();

            foreach (var report in reports.Skip(1))
            {
                cursor.Row(initRow)
                    .Print(report.FileName).Merge(2)
                    .Down()
                    .Print("Values", "Change")
                    .Down()
                    .PrintDown(report.Fields.Select(x => new TypedAmount {Amount = x.CurrentSum, Type = x.Type}))
                    .Right()
                    .PrintDown(report.Fields.Select(x => new TypedAmount {Amount = x.Change, Type = DType.Percent}), StyleConditions.ChangePercent)
                    .Right();
            }

            cursor.Down(first.Fields.Count);
        }

        private void printStates(IList<CompareSubReport> reports, ExcelCursor cursor)
        {
            var first = reports.First();
            cursor.Print("State:", first.State).Down();

            var initRow = cursor.Pos.Row;

            cursor.Print("", first.FileName)
                .Down()
                .PrintDown(first.Fields.Select(x => x.Title))
                .Right()
                .PrintDown(first.Fields.Select(x => new TypedAmount {Amount = x.CurrentSum, Type = x.Type}))
                .Right();

            foreach (var report in reports.Skip(1))
            {
                cursor
                    .Row(initRow)
                    .Print(report.FileName).Merge(2)
                    .Down()
                    .PrintDown(report.Fields.Select(x => new TypedAmount { Amount = x.CurrentSum, Type = x.Type }))
                    .Right()
                    .PrintDown(report.Fields.Select(x => new TypedAmount {Amount = x.Change, Type = DType.Percent}), StyleConditions.ChangePercent)
                    .Right();
            }

            cursor.Down(first.Fields.Count);
        }
    }
}
