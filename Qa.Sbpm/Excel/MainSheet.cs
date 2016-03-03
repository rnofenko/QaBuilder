﻿using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using Qa.Excel;
using Qa.Sbpm.Compare;

namespace Qa.Sbpm.Excel
{
    public class MainSheet
    {
        public void Print(ComparePacket packet, ExcelWorksheet sheet)
        {
            const int initColumn = 2;
            var cursor = new ExcelCursor(sheet);
            new Header().Print(cursor, packet.Strucure.Name);
            
            cursor.Column(initColumn).Row(5);
            printTotal(packet.Reports.Select(x => x.GetSubReport(QaSettings.National)).ToList(), cursor);
            cursor.Down(2);

            foreach (var state in packet.States.Where(x => x != QaSettings.National))
            {
                cursor.Column(initColumn);
                printState(packet.GetTransformedSubReports(QaSettings.TransformByState, state), cursor);
                cursor.Down(2);
            }
            
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(1).Width = 3;
        }

        private void printTotal(IList<CompareSubReport> reports, ExcelCursor cursor)
        {
            var initRow = cursor.GetRow();
            var isFirst = true;
            foreach (var file in reports)
            {
                cursor.Row(initRow);

                if (isFirst)
                {
                    cursor.Value("", file.FileName)
                        .Down()
                        .Value("Field", "Values");
                }
                else
                {
                    cursor.Value(file.FileName).Merge(2)
                        .Down()
                        .Value("Values", "Change, %");
                }
                
                var startRow = cursor.GetRow();
                if (isFirst)
                {
                    foreach (var field in file.Fields)
                    {
                        cursor.Down().Value(field.Title);
                    }
                    cursor.NextColumn();
                }

                cursor.Row(startRow);
                foreach (var field in file.Fields)
                {
                    cursor.Down().Value(field.CurrentSum, field.Type);
                }
                cursor.NextColumn();

                if (!isFirst)
                {
                    cursor.Row(startRow);
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
        }

        private void printState(IList<CompareSubReport> reports, ExcelCursor cursor)
        {
            cursor.Value("State:", reports.First().State).Down();

            var initRow = cursor.GetRow();
            var isFirst = true;
            foreach (var report in reports)
            {
                cursor.Row(initRow);

                if (isFirst)
                {
                    cursor.Value("", report.FileName)
                        .Down()
                        .Value("Field", "Values");
                }
                else
                {
                    cursor.Value(report.FileName).Merge(2)
                        .Down()
                        .Value("Values", "Change, %");
                }
                
                var startRow = cursor.GetRow();
                if (isFirst)
                {
                    foreach (var field in report.Fields)
                    {
                        cursor.Down().Value(field.Title);
                    }
                    cursor.NextColumn();
                }

                cursor.Row(startRow);
                foreach (var field in report.Fields)
                {
                    cursor.Down().Value(field.CurrentSum, field.Type);
                }
                cursor.NextColumn();

                if (!isFirst)
                {
                    cursor.Row(startRow);
                    foreach (var field in report.Fields)
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
        }
    }
}
