using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Qa.Compare;

namespace Qa.Excel
{
    public class OpenXmlExporterToExcel: IExcelExporter
    {
        public void Export(List<ComparePacket> packets, CompareSettings settings)
        {
            using (var workbook = SpreadsheetDocument.Create("comparing.xlsx", SpreadsheetDocumentType.Workbook))
            {
                workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new Workbook {Sheets = new Sheets()};

                foreach (var packet in packets)
                {
                    var sheetData = createSheet(workbook, packet);
                    fill(packet, sheetData);
                }
            }
        }

        private void fill(ComparePacket packet, SheetData sheet)
        {
            var first = packet.Files.First();
            sheet.AppendChild(new Row());

            var titleRow = createRow(sheet);
            var headerRow = createRow(sheet);
            var rowsCountRow = createRow(sheet);
            var fieldRows = first.Fields.Select(x => createRow(sheet)).ToList();
            
            titleRow.Add("").Add(first.FileName);
            headerRow.Add("").Add("Value").Add("");
            rowsCountRow.Add("Rows").Add(first.RowsCount.Current).Add("");
            for (var i = 0; i < first.Fields.Count; i++)
            {
                var row = fieldRows[i];
                var field = first.Fields[i];
                row.Add(field.Name).Add(field.CurrentSum).Add("");
            }

            foreach (var file in packet.Files.Skip(1))
            {
                titleRow.Add("").Add(file.FileName).Add("");
                headerRow.Add("Value").Add("Change, %").Add("");
                rowsCountRow.Add(file.RowsCount.Current).Add(file.RowsCount.Increase).Add("");

                for (var i = 0; i < file.Fields.Count; i++)
                {
                    var row = fieldRows[i];
                    var field = file.Fields[i];
                    row.Add(field.CurrentSum).Add(field.Change).Add("");
                }
            }
        }

        private Row createRow(SheetData sheet)
        {
            var row = new Row();
            row.AppendChild(new Cell());
            sheet.AppendChild(row);
            return row;
        }

        private SheetData createSheet(SpreadsheetDocument workbook, ComparePacket packet)
        {
            var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            sheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            var relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            var sheet = new Sheet
            {
                Id = relationshipId,
                SheetId = sheetId,
                Name = packet.Strucure.Name
            };
            sheets.Append(sheet);

            return sheetData;
        }
    }
}