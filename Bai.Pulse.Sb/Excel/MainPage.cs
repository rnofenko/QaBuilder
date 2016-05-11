using System.Linq;
using OfficeOpenXml;
using Q2.Core.Compare;
using Q2.Core.Excel;
using Q2.Core.Structure;

namespace Q2.Bai.Pulse.Sb.Excel
{
    public class MainPage : IExportPage
    {
        private readonly CommonPageSettings _settings;
        private readonly GroupedFieldPrinter _groupedFieldPrinter;
        private readonly GroupOfNumberFieldPrinter _groupOfNumberFieldPrinter;
        private const int INIT_COLUMN = 2;

        public MainPage()
        {
            _settings = new CommonPageSettings {Freeze = false};
            _groupedFieldPrinter = new GroupedFieldPrinter();
            _groupOfNumberFieldPrinter = new GroupOfNumberFieldPrinter();
        }

        public void Print(ComparePacket packet, ExcelWorksheet sheet)
        {
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Structure.Name);
            cursor.Column(INIT_COLUMN).Row(5);

            _groupOfNumberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation != CalculationType.CountUnique), cursor, packet);
            _groupOfNumberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation == CalculationType.CountUnique), cursor, packet);

            foreach (var field in packet.GroupedFields)
            {
                _groupedFieldPrinter.Print(field, cursor, packet);
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
            sheet.Column(packet.Files.Count * 2 + 2).Width = 3;

            if (_settings.Freeze)
            {
                cursor.Sheet.View.FreezePanes(4, 3);
            }
        }
    }
}