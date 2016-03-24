using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.Excel;

namespace Qa.Bai.Benchmark.Sb
{
    public class MainSheet : IExportPage
    {
        internal const int InitColumn = 2;

        public void PrintReport(ComparePacket packet, ExcelWorksheet sheet)
        {
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Structure.Name);
            
            cursor.Column(InitColumn).Row(5);
            new ContentGenerator().Print(packet, cursor, InitColumn);
            cursor.Down(2);
            
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }
    }
}
