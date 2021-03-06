using System.Linq;
using OfficeOpenXml.Style;
using Qa.Core.Compare;
using Qa.Core.Parsers;

namespace Qa.Core.Excel
{
    public class FourMonthsGroupedFieldPrinter : IGroupedFieldPrinter
    {
        private readonly FileDateFormatter _dateParser;

        public FourMonthsGroupedFieldPrinter()
        {
            _dateParser = new FileDateFormatter();
        }

        public void Print(GroupedField field, ExcelCursor cursor, ComparePacket packet)
        {
            var startColumn = cursor.Pos.Column;
            var first = packet.Files.First();

            cursor.Column(startColumn)
                .Header(field.Title)
                .TopLeftBorderCorner()
                .MergeDown(2)
                .Right()
                .HeaderDown(_dateParser.Format(first.FileName), "Values")
                .Right();

            foreach (var report in packet.Files.Skip(1))
            {
                cursor
                    .TopLeftBorderCorner()
                    .Header(_dateParser.Format(report.FileName))
                    .Merge(2)
                    .Down()
                    .Header("Abs. Change", "Change in %")
                    .Right(2)
                    .Up();
            }

            cursor.Down();

            foreach (var key in field.Keys)
            {
                cursor.Down()
                    .Column(startColumn)
                    .Print(key, field.Qa.Style)
                    .Right();

                foreach (var file in packet.Files)
                {
                    cursor
                        .PrintIf(file == first, field.GetCurrent(file, key))
                        .PrintIf(file != first, field.GetAbsChange(file, key))
                        .RightIf(file != first)
                        .PrintIf(file != first, field.GetPercentChange(file, key), StyleConditions.ChangePercent)
                        .DrawBorder(ExcelBorderStyle.Thick, key == field.Keys.Last())
                        .Right();
                }
            }

            cursor.Column(startColumn).Down(3);
        }
    }
}