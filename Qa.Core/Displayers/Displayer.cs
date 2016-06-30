using System;
using System.Collections.Generic;
using Qa.Core.Parsers.FileReaders;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Displayers
{
    public class Displayer
    {
        private const int PORTION_SIZE = 10;

        public void DisplayColumn(string filepath, FileStructure structure, int columnIndex)
        {
            using (var reader = FileReaderFactory.Create(filepath, structure.Qa.GetLineParser()))
            {
                reader.Skip(structure.Qa.RowsInHeader);
                var rowNumber = 0;
                var rowNumberInPortion = 0;
                while (true)
                {
                    var row = reader.ParseNextRow();
                    if (row == null)
                    {
                        Lo.Wl("End of the file. Press any key to continue...");
                        Console.ReadKey();
                        return;
                    }
                    rowNumber++;
                    rowNumberInPortion++;
                    Lo.Wl(row[columnIndex]);
                    if (rowNumberInPortion == PORTION_SIZE)
                    {
                        rowNumberInPortion = 0;
                        Lo.W(rowNumber.ToString(), ConsoleColor.Yellow).Wl(" rows were displayed. Press SPACE to show next " + PORTION_SIZE);
                        if (!toBeContinue())
                        {
                            return;
                        }
                    }
                }
            }
        }

        public void DisplayByRows(string filepath, FileStructure structure)
        {
            using (var reader = FileReaderFactory.Create(filepath, structure.Qa.GetLineParser()))
            {
                reader.Skip(structure.Qa.RowsInHeader);
                var rowNumber = 0;
                var rowNumberInPortion = 0;
                while (true)
                {
                    var row = reader.ReadNextRow();
                    if (row == null)
                    {
                        Lo.Wl("End of the file. Press any key to continue...");
                        Console.ReadKey();
                        return;
                    }
                    rowNumber++;
                    rowNumberInPortion++;
                    Lo.Wl(row);
                    if (rowNumberInPortion == PORTION_SIZE)
                    {
                        rowNumberInPortion = 0;
                        Lo.W(rowNumber.ToString(), ConsoleColor.Yellow).Wl(" rows were displayed. Press SPACE to show next " + PORTION_SIZE);
                        if (!toBeContinue())
                        {
                            return;
                        }
                    }
                }
            }
        }

        private bool toBeContinue()
        {
            try
            {
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        return false;
                    }
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        return true;
                    }
                }
            }
            finally
            {
                Lo.Wl();
            }
        }

        public void SplitRowsByFields(string filePath, FileStructure structure)
        {
            using (var reader = FileReaderFactory.Create(filePath, structure.Qa.GetLineParser()))
            {
                var rowNumber = 0;
                while (true)
                {
                    var row = reader.ParseNextRow();
                    if (row == null)
                    {
                        Lo.Wl("End of the file. Press any key to continue...");
                        Console.ReadKey();
                        return;
                    }
                    rowNumber++;
                    showLine(row, structure.Fields);

                    Lo.W(rowNumber.ToString(), ConsoleColor.Yellow).Wl(" rows were displayed. Press SPACE to show next row.");
                    if (!toBeContinue())
                    {
                        return;
                    }
                }
            }
        }

        private void showLine(string[] row, IList<Field> fields)
        {
            var i = 0;
            while (i < 200)
            {
                string fieldName = null;
                if (fields.Count > i)
                {
                    fieldName = fields[i].Name.PadRight(30);
                }
                string value = null;
                if (row.Length > i)
                {
                    value = row[i];
                }
                if (fieldName == null && value == null)
                {
                    return;
                }

                i++;
                Lo.Wl(string.Format("{0}. {1} - {2}", i.ToString().PadLeft(2), fieldName, value));
            }
        }
    }
}
