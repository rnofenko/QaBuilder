using System;
using System.IO;
using System.Linq;
using Qa.Core.Parsers.FileReaders;
using Qa.Core.Selectors;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Editors
{
    public class EditPrompt
    {
        private readonly FileSelector _fileSelector;
        private readonly StructureSelector _structureSelector;

        public EditPrompt()
        {
            _fileSelector = new FileSelector();
            _structureSelector = new StructureSelector();
        }

        public void Start(Settings settings)
        {
            Lo.Wl().Wl()
                .Wl(string.Format("..................... Editor - {0} .........................", settings.Project))
                .Wl(string.Format("Current folder is {0}", settings.WorkingFolder));

            while (true)
            {
                Lo.Wl()
                    .Wl("Select command:")
                    .Wl("1. Add column")
                    .Wl("2. Delete column");

                var key = Console.ReadKey();
                if (key.KeyChar == '1')
                {
                    addColumn(settings);
                }
                else if (key.KeyChar == '2')
                {
                    deleteColumn(settings);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

        private void addColumn(Settings settings)
        {
            var file = _fileSelector.SelectAnyFile(settings.WorkingFolder);
            var structure = _structureSelector.Select(settings);

            Lo.Wl().Wl("Select position for new column. 0 - the first, 999 - at the end:");
            var positionStr = Console.ReadLine();
            int position;
            if (!int.TryParse(positionStr, out position))
            {
                Lo.Wl("Position can be only number from 0 to 999.", ConsoleColor.Red);
                return;
            }
            Lo.Wl().Wl("Input default value:");
            var defaultValue = Console.ReadLine();

            Lo.Wl().W("New column will be added in file ").Wl(file,ConsoleColor.Yellow)
                .W("for structure ").Wl(structure.Qa.Name,ConsoleColor.Yellow)
                .W("on position ").Wl(position.ToString(), ConsoleColor.Yellow)
                .W("with default value ").Wl(defaultValue, ConsoleColor.Yellow)
                .Wl("Press ENTER to start.");
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                Lo.Wl("Adding new column was canceled.", ConsoleColor.Cyan);
            }

            addColumn(file, structure.Qa, position, defaultValue);
        }

        private void deleteColumn(Settings settings)
        {
            var file = _fileSelector.SelectAnyFile(settings.WorkingFolder);
            var structure = _structureSelector.Select(settings);

            Lo.Wl().Wl("Select position for new column. 0 - the first, 999 - at the end:");
            var positionStr = Console.ReadLine();
            int position;
            if (!int.TryParse(positionStr, out position))
            {
                Lo.Wl("Position can be only number from 0 to 999.", ConsoleColor.Red);
                return;
            }
            
            Lo.Wl().W("Column will be deleted from file ").Wl(file, ConsoleColor.Yellow)
                .W("for structure ").Wl(structure.Qa.Name, ConsoleColor.Yellow)
                .W("on position ").Wl(position.ToString(), ConsoleColor.Yellow)
                .Wl("Press ENTER to start.");
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                Lo.Wl("Deleting column was canceled.", ConsoleColor.Cyan);
            }

            deleteColumn(file, structure.Qa, position);
        }

        private void processFile(string sourceFile, IStructure structure, Func<string[],string> rowAction)
        {
            using (var reader = FileReaderFactory.Create(sourceFile, structure.GetLineParser()))
            {
                using (var writer = new StreamWriter(sourceFile + "." + DateTime.Now.Ticks))
                {
                    for (var i = 0; i < structure.SkipRows; i++)
                    {
                        writer.WriteLine(reader.ReadNextRow());
                    }

                    string[] row;
                    while ((row = reader.ParseNextRow()) != null)
                    {
                        var rebuiltRow = rowAction(row);
                        writer.WriteLine(rebuiltRow);

                        if (reader.RowNumber % 500000 == 0)
                        {
                            Lo.Wl(string.Format("Processed {0} rows.", reader.RowNumber));
                        }
                    }
                }
                Lo.Wl(string.Format("Processed {0} rows.", reader.RowNumber));
            }
        }

        private void addColumn(string sourceFile, IStructure structure, int position, string defaultValue)
        {
            processFile(sourceFile, structure, row =>
            {
                var pos = position > row.Length ? row.Length : position;
                var list = row.ToList();
                list.Insert(pos, defaultValue);
                return string.Join(structure.Delimiter, list);
            });
        }

        private void deleteColumn(string sourceFile, IStructure structure, int position)
        {
            processFile(sourceFile, structure, row =>
            {
                var pos = position > row.Length ? row.Length : position;
                var list = row.ToList();
                list.RemoveAt(pos);
                return string.Join(structure.Delimiter, list);
            });
        }
    }
}
