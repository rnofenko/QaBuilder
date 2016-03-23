using System.Linq;
using OfficeOpenXml;
using Qa.Bai.Pulse.Sb.Compare;
using Qa.Bai.Sbp;
using Qa.Bai.Sbp.Compare;
using Qa.Bai.Sbp.Excel;
using Qa.Core.Excel;

namespace Qa.Bai.Pulse.Sb.Excel
{
    public class PeriodSheet
    {
        public void Print(PulseComparePacket packet, PulseCompareReport report, ExcelWorksheet sheet)
        {
            const int initColumn = 2;
            const int initRow = 5;
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Strucure.Name);

            cursor.Column(initColumn).Row(initRow);
            printFieldNames(report.GetSubReport(QaSettings.National), cursor);

            cursor.Right().Row(initRow);
            printState(report.GetSubReport(QaSettings.National), cursor);

            foreach (var state in packet.States.Where(x => x != QaSettings.National))
            {
                cursor.Right().Row(initRow);
                printState(report.GetSubReport(state), cursor);
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
        }

        private void printFieldNames(CompareSubReport report, ExcelCursor cursor)
        {
            cursor
                .Down()
                .TopLeftBorderCorner()
                .PrintDown(report.Fields.Select(x => x.Title))
                .Down(report.Fields.Count - 1)
                .DrawBorder();
        }

        private void printState(CompareSubReport report, ExcelCursor cursor)
        {
            cursor
                .Print(report.State).BackgroundColor(QaColor.HeaderBackground).DrawBorder().Center()
                .Down()
                .TopLeftBorderCorner()
                .PrintDown(report.Fields.Select(x => x.GetCurrent()))
                .Down(report.Fields.Count - 1)
                .DrawBorder();
        }
    }
}