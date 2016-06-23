using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml.Style;
using Qa.Core.Compare;
using Qa.Core.Parsers;

namespace Qa.Core.Excel
{
    public class FourMonthsNumberFieldPrinter: INumberFieldPrinter
    {
        private readonly FileDateFormatter _dateParser;

        public FourMonthsNumberFieldPrinter()
        {
            _dateParser = new FileDateFormatter();
        }

        public void Print(IEnumerable<NumberField> fields, ExcelCursor cursor, ComparePacket packet)
        {
            var fieldsList = fields.ToList();
            if (fieldsList.IsEmpty())
            {
                return;
            }

            var startColumn = cursor.Pos.Column;
            var first = packet.Files.First();

            cursor.Column(startColumn)
                .Header("Field")
                .TopLeftBorderCorner()
                .MergeDown(2)
                .Right()
                .HeaderDown(_dateParser.Format(first.FileName), "Values")
                .Right();

            foreach (var file in packet.Files.Skip(1))
            {
                cursor
                    .TopLeftBorderCorner()
                    .Header(_dateParser.Format(file.FileName))
                    .Merge(2)
                    .Down()
                    .Header("Abs. Change", "Change in %")
                    .Right(2)
                    .Up();
            }

            cursor.Down();

            foreach (var field in fieldsList)
            {
                cursor.Down()
                    .Column(startColumn)
                    .Print(field.Title, field.Style)
                    .Right();

                foreach (var file in packet.Files)
                {
                    cursor
                        .PrintIf(file == first, field.GetCurrent(file))
                        .PrintIf(file != first, field.GetAbsChange(file))
                        .RightIf(file != first)
                        .PrintIf(file != first, field.GetPercentChange(file), StyleConditions.ChangePercent)
                        .DrawBorder(ExcelBorderStyle.Thick, field == fieldsList.Last())
                        .Right();
                }
            }

            cursor.Column(startColumn).Down(3);
        }
    }
}