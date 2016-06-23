using System.Linq;
using OfficeOpenXml.Style;
using Qa.Core.Compare;
using Qa.Core.Parsers;

namespace Qa.Core.Excel
{
    public class GroupedFieldPrinter
    {
        private readonly FileDateFormatter _dateParser;

        public GroupedFieldPrinter()
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
                    .Header("Values", "Change")
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
                        .Print(field.GetCurrent(file, key))
                        .RightIf(file != first)
                        .PrintIf(file != first, field.GetChange(file, key), StyleConditions.ChangePercent)
                        .DrawBorder(ExcelBorderStyle.Thick, key == field.Keys.Last())
                        .Right();
                }
            }

            cursor.Column(startColumn).Down(3);
        }
    }
}