using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml.Style;
using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public class GroupOfNumberFieldPrinter
    {
        private readonly FileDateParser _dateParser;

        public GroupOfNumberFieldPrinter()
        {
            _dateParser = new FileDateParser();
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
                .HeaderDown(_dateParser.ExtractDate(first.FileName), "Values")
                .Right();

            foreach (var file in packet.Files.Skip(1))
            {
                cursor
                    .TopLeftBorderCorner()
                    .Header(_dateParser.ExtractDate(file.FileName))
                    .Merge(2)
                    .Down()
                    .Header("Values", "Change")
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
                        .Print(field.GetCurrent(file))
                        .RightIf(file != first)
                        .PrintIf(file != first, field.GetChange(file), StyleConditions.ChangePercent)
                        .DrawBorder(ExcelBorderStyle.Thick, field == fieldsList.Last())
                        .Right();
                }
            }

            cursor.Column(startColumn).Down(3);
        }
    }
}