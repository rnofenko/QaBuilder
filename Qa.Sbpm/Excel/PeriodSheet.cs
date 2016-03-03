using System;
using System.Linq;
using OfficeOpenXml;
using Qa.Excel;
using Qa.Sbpm.Compare;

namespace Qa.Sbpm.Excel
{
    public class PeriodSheet
    {
        public void Print(ComparePacket packet, CompareReport report, ExcelWorksheet sheet)
        {
            const int initColumn = 2;
            const int initRow = 5;
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Strucure.Name);

            cursor.Column(initColumn).Row(initRow);
            printFieldNames(report.GetSubReport(QaSettings.National), cursor);

            cursor.NextColumn().Row(initRow);
            printState(report.GetSubReport(QaSettings.National), cursor);

            foreach (var state in packet.States.Where(x=>x!=QaSettings.National))
            {
                cursor.NextColumn().Row(initRow);
                printState(report.GetSubReport(state), cursor);
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
        }

        private void printFieldNames(CompareSubReport report, ExcelCursor cursor)
        {
            cursor.Value("Field");
            foreach (var field in report.Fields)
            {
                cursor.Down().Value(field.Title);
            }
        }

        private void printState(CompareSubReport report, ExcelCursor cursor)
        {
            cursor.Value(report.State);
            foreach (var field in report.Fields)
            {
                cursor.Down().Value(field.CurrentSum, field.Type);
            }
        }
    }
}
