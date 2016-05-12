using System.Drawing;
using System.Linq;
using OfficeOpenXml;
using Q2.Core.Compare;
using Q2.Core.Excel;
using Q2.Core.Structure;

namespace Q2.Bai.Pulse.Sb.Monthly.Excel
{
    public class MainPage : IExportPage
    {
        private readonly GroupedFieldPrinter _groupedFieldPrinter;
        private readonly GroupOfNumberFieldPrinter _groupOfNumberFieldPrinter;
        private ExcelCursor _cursor;
        private const int INIT_COLUMN = 2;

        public MainPage()
        {
            _groupedFieldPrinter = new GroupedFieldPrinter();
            _groupOfNumberFieldPrinter = new GroupOfNumberFieldPrinter();
        }

        public void Print(ComparePacket packet, ExcelWorksheet sheet)
        {
            _cursor = new ExcelCursor(sheet);
            new Header().Print(_cursor, packet.Structure.Name);
            _cursor.Column(INIT_COLUMN).Row(5);

            _groupOfNumberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation != CalculationType.CountUnique), _cursor, packet);
            _groupOfNumberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation == CalculationType.CountUnique), _cursor, packet);

            foreach (var field in packet.GroupedFields)
            {
                _groupedFieldPrinter.Print(field, _cursor, packet);
            }
        }

        public void PrintState(ComparePacket packet, ExcelWorksheet sheet)
        {
            _cursor.Print(packet.SplitValue).BackgroundColor(Color.Bisque).Down();
            _groupOfNumberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation != CalculationType.CountUnique), _cursor, packet);
            _groupOfNumberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation == CalculationType.CountUnique), _cursor, packet);

            foreach (var field in packet.GroupedFields)
            {
                _groupedFieldPrinter.Print(field, _cursor, packet);
            }
        }

        public void Footer(ExcelWorksheet sheet)
        {
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }
    }
}