using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Qa.Compare;
using Qa.Properties;

namespace Qa.Excel
{
    public class EpExporterToExcel : IExcelExporter
    {
        public void Export(List<ComparePacket> packets, CompareSettings settings)
        {
            var path = Path.Combine(settings.WorkingFolder, "comparing.xlsx");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var file = new FileInfo(path);
            using (var package = new ExcelPackage(file))
            {
                foreach (var packet in packets)
                {
                    var sheet = package.Workbook.Worksheets.Add(packet.Strucure.Name);
                    fill(packet, sheet);
                }
                package.Save();
            }
            Process.Start(path);
        }

        private void putHeader(ExcelCursor cursor)
        {
            var logo = (Image)Resources.ResourceManager.GetObject("SantanderLogo");
            var logoCell = cursor.Sheet.Drawings.AddPicture("Logo", logo);
            logoCell.SetPosition(0, 0, 0, 0);
            logoCell.SetSize(160, 40);

            for (var rowNumber = 1; rowNumber < 3; rowNumber++)
            {
                var row = cursor.Sheet.Row(rowNumber);
                row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 254, 28, 25));
            }
        }

        private void fill(ComparePacket packet, ExcelWorksheet sheet)
        {
            var cursor = new ExcelCursor(sheet).SetColumn(2);
            putHeader(cursor);
            
            var isFirst = true;
            foreach (var file in packet.Files)
            {
                cursor.Row(5);
                if (isFirst)
                {
                    cursor
                        .Value("", file.FileName)
                        .Down()
                        .Value("", "Values");
                }
                else
                {
                    cursor
                        .Value(file.FileName)
                        .Down()
                        .Value("Values", "Change, %");
                }
                cursor.Down();

                var startRow = cursor.GetRow();
                if (isFirst)
                {
                    cursor.Value("Rows");
                    foreach (var field in file.Fields)
                    {
                        cursor.Down().Value(field.Name);
                    }
                    cursor.NextColumn();
                }

                cursor.Row(startRow).Value(file.RowsCount.Current);
                foreach (var field in file.Fields)
                {
                    cursor.Down().Value(field.CurrentSum, field.Type);
                }
                cursor.NextColumn();
                
                if (!isFirst)
                {
                    cursor.Row(startRow).Percent(file.RowsCount.Increase);
                    foreach (var field in file.Fields)
                    {
                        cursor.Down().Percent(field.Change);
                        if (Math.Abs(field.Change) > 0.35)
                        {
                            cursor.SetAsDanger();
                        }
                        else if (Math.Abs(field.Change) > 0.20)
                        {
                            cursor.SetAsWarning();
                        }
                    }
                    cursor.NextColumn();
                }

                if (isFirst)
                {
                    isFirst = false;
                }

                cursor.NextColumn();
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
        }
    }
}