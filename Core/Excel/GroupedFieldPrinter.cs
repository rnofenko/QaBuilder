using System.Linq;
using OfficeOpenXml.Style;
using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public class GroupedFieldPrinter
    {
        private readonly DateParser _dateParser;

        public GroupedFieldPrinter()
        {
            _dateParser = new DateParser();
        }

        public void Print(GroupedField field, ExcelCursor cursor, ComparePacket packet)
        {
            var startColumn = cursor.Pos.Column;
            var first = packet.Reports.First();

            cursor.Column(startColumn)
                .Header(field.Title)
                .TopLeftBorderCorner()
                .MergeDown(2)
                .Right()
                .HeaderDown(_dateParser.ExtractDate(first.FileName), "Values")
                .Right();

            foreach (var report in packet.Reports.Skip(1))
            {
                cursor
                    .TopLeftBorderCorner()
                    .Header(_dateParser.ExtractDate(report.FileName))
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
                    .Print(field.GetTranslate(key))
                    .Right();

                foreach (var file in packet.Reports)
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