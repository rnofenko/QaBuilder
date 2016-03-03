using System.Linq;
using OfficeOpenXml;
using Qa.BAI_DPB.Compare;
using Qa.Core.Excel;

namespace Qa.BAI_DPB.Excel
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
            cursor.Print("Field");
            foreach (var field in report.Fields)
            {
                cursor.Down().Print(field.Title);
            }
        }

        private void printState(CompareSubReport report, ExcelCursor cursor)
        {
            cursor.Print(report.State);
            foreach (var field in report.Fields)
            {
                cursor.Down().Print(field.CurrentSum, field.Type);
            }
        }
    }
}
