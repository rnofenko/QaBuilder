using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml.Style;
using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public class GroupOfNumberFieldPrinter
    {
        private readonly DateParser _dateParser;

        public GroupOfNumberFieldPrinter()
        {
            _dateParser = new DateParser();
        }

        public void Print(IEnumerable<NumberField> fields, ExcelCursor cursor, ComparePacket packet)
        {
            var fieldsList = fields.ToList();
            if (!fieldsList.Any())
            {
                return;
            }

            var startColumn = cursor.Pos.Column;
            cursor
                .Column(startColumn)
                .TopLeftBorderCorner()
                .Header("")
                .Right()
                .Header(packet.Reports.Select(x => _dateParser.ExtractDate(x.FileName)).First())
                .Down()
                .Left()
                .Header("", "Values")
                .Right();

            foreach (var report in packet.Reports.Skip(1))
            {
                cursor
                    .Up()
                    .Right()
                    .Header(_dateParser.ExtractDate(report.FileName))
                    .Merge(2)
                    .TopLeftBorderCorner()
                    .Down()
                    .Header("Values", "Change")
                    .Right();
            }

            foreach (var field in fieldsList)
            {
                cursor
                    .Column(startColumn)
                    .Down()
                    .Print(field.Title)
                    .Right()
                    .Integer(field.Numbers.First().Current);

                foreach (var compareNumber in field.Numbers.Skip(1))
                {
                    cursor
                        .Right()
                        .Integer(compareNumber.Current)
                        .Right()
                        .Percent(compareNumber.Change, StyleConditions.ChangePercent)
                        .DrawBorder(ExcelBorderStyle.Thick, field == packet.NumberFields.Last());
                }
            }

            cursor
                .DrawBorder(ExcelBorderStyle.Thick)
                .Down(3)
                .Column(startColumn);
        }
    }
}