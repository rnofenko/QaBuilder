using System.Drawing;
using System.Linq;
using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Bai.Pulse.Excel
{
    public class PulsePage : IExportPage
    {
        private ExcelCursor _cursor;
        private readonly FieldPrinterFactory _factory;
        private const int INIT_COLUMN = 2;

        public PulsePage()
        {
            _factory = new FieldPrinterFactory();
        }

        public void Print(string structureName, ComparePacket packet, ExcelWorksheet sheet)
        {
            _cursor = new ExcelCursor(sheet);
            new Header().Print(_cursor, structureName);
            _cursor.Column(INIT_COLUMN).Row(5);

            var numberFieldPrinter = _factory.CreateNumberPrinter(packet.CompareMethod);
            numberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation != CalculationType.CountUnique), _cursor, packet);
            numberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation == CalculationType.CountUnique), _cursor, packet);

            var groupedFieldPrinter = _factory.CreateGroupedPrinter(packet.CompareMethod);
            foreach (var field in packet.GroupedFields)
            {
                groupedFieldPrinter.Print(field, _cursor, packet);
            }
        }

        public void PrintState(ComparePacket packet, ExcelWorksheet sheet)
        {
            _cursor.Print(packet.SplitValue).BackgroundColor(Color.Bisque).Down();
            var numberFieldPrinter = _factory.CreateNumberPrinter(packet.CompareMethod);
            numberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation != CalculationType.CountUnique), _cursor, packet);
            numberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation == CalculationType.CountUnique), _cursor, packet);

            var groupedFieldPrinter = _factory.CreateGroupedPrinter(packet.CompareMethod);
            foreach (var field in packet.GroupedFields)
            {
                groupedFieldPrinter.Print(field, _cursor, packet);
            }
        }

        public void Footer(ExcelWorksheet sheet)
        {
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }
    }
}