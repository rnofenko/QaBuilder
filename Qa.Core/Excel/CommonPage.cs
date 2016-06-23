using System.Linq;
using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public class CommonPage: IExportPage
    {
        private readonly CommonPageSettings _settings;
        private readonly FieldPrinterFactory _factory;
        private const int INIT_COLUMN = 2;

        public CommonPage()
        {
            _factory = new FieldPrinterFactory();
            _settings = new CommonPageSettings();
        }

        public CommonPage(CommonPageSettings settings)
            :this()
        {
            _settings = settings;
        }

        public void Print(string structureName, ComparePacket packet, ExcelWorksheet sheet)
        {
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, structureName);
            cursor.Column(INIT_COLUMN).Row(5);

            var numberFieldPrinter = _factory.CreateNumberPrinter(packet.CompareMethod);
            numberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation != CalculationType.CountUnique), cursor, packet);
            numberFieldPrinter.Print(packet.NumberFields.Where(x => x.Calculation == CalculationType.CountUnique), cursor, packet);

            var groupedFieldPrinter = _factory.CreateGroupedPrinter(packet.CompareMethod);
            foreach (var field in packet.GroupedFields)
            {
                groupedFieldPrinter.Print(field, cursor, packet);
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
            sheet.Column(packet.Files.Count * 2 + 2).Width = 3;

            if (_settings.Freeze)
            {
                cursor.Sheet.View.FreezePanes(4, 3);
            }

            sheet.Cells[1,1, sheet.Cells.Rows,sheet.Cells.Columns].AutoFitColumns(3,30);
        }
    }
}