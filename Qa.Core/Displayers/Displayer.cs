using System;
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
            using (var reader = FileReaderFactory.Create(filepath, structure.Qa))
            {
                reader.Skip(structure.Qa.RowsInHeader);
                int rowNumber = 0;
                int rowNumberInPortion = 0;
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
            using (var reader = FileReaderFactory.Create(filepath, structure.Qa))
            {
                reader.Skip(structure.Qa.RowsInHeader);
                int rowNumber = 0;
                int rowNumberInPortion = 0;
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
    }
}
