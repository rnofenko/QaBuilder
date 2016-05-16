using System.Linq;
using OfficeOpenXml;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public class CommonPage: IExportPage
    {
        private readonly CommonPageSettings _settings;
        private readonly GroupedFieldPrinter _groupedFieldPrinter;
        private readonly GroupOfNumberFieldPrinter _groupOfNumberFieldPrinter;
        private const int INIT_COLUMN = 2;

        public CommonPage()
        {
            _settings = new CommonPageSettings();
            _groupedFieldPrinter = new GroupedFieldPrinter();
            _groupOfNumberFieldPrinter = new GroupOfNumberFieldPrinter();
        }

        public CommonPage(CommonPageSettings settings)
            :this()
        {
            _settings = settings;
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