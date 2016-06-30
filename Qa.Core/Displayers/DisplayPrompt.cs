using System;
using System.IO;
using System.Linq;
using Qa.Core.Prompts;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Displayers
{
    public class DisplayPrompt
    {
        private readonly QaFileFinder _fileFinder;
        private readonly SelectorPrompt _selector;
        private readonly Displayer _displayer;
        private readonly PathFinder _pathFinder;

        public DisplayPrompt()
        {
            _fileFinder = new QaFileFinder();
            _selector = new SelectorPrompt();
            _displayer = new Displayer();
            _pathFinder = new PathFinder();
        }

        public void Start(Settings settings)
        {
            Lo.Wl().Wl()
                .Wl(string.Format("..................... Displayer - {0} .........................", settings.Project))
                .Wl(string.Format("Current folder is {0}", settings.WorkingFolder));
            
            while (true)
            {
                Lo.Wl()
                    .Wl("Select command:")
                    .Wl("1. Show one column")
                    .Wl("2. Show whole rows")
                    .Wl("3. Show file's rows by fields");
                
                var key = Console.ReadKey();
                if (key.KeyChar == '1')
                {
                    showOneColumn(settings);
                }
                else if (key.KeyChar == '2')
                {
                    showWholeRows(settings);
                }
                else if (key.KeyChar == '3')
                {
                    splitRowByField(settings);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

        private void splitRowByField(Settings settings)
        {
            var selectedStructure = selectStructure(settings);
            var file = selectAnyFile(settings);
            if (file == null || selectedStructure == null)
            {
                return;
            }
            
            _displayer.SplitRowsByFields(file, selectedStructure);
        }

        private void showOneColumn(Settings settings)
        {
            var selectedStructure = selectStructure(settings);
            var selectedFile = selectProjectFile(settings, selectedStructure);
            if (selectedFile == null)
            {
                return;
            }
            var column = _selector.Select(selectedStructure.Fields.Select(x => x.Name), "Select field");
            if (column >= 0)
            {
                _displayer.DisplayColumn(selectedFile, selectedStructure, column);
            }
        }

        private void showWholeRows(Settings settings)
        {
            var selectedStructure = selectStructure(settings);
            var selectedFile = selectAnyFile(settings);
            if (selectedFile == null || selectedStructure == null)
            {
                return;
            }
            _displayer.DisplayWholeRows(selectedFile, selectedStructure);
        }

        private string selectAnyFile(Settings settings)
        {
            var filePaths = _pathFinder.Find(settings.WorkingFolder);
            var fileIndex = _selector.Select(filePaths.Select(Path.GetFileName), "Select file for check");
            if (fileIndex < 0)
            {
                return null;
            }
            return filePaths[fileIndex];
        }

        private string selectProjectFile(Settings settings, FileStructure structure)
        {
            if (structure == null)
            {
                return null;
            }

            var files = _fileFinder.Find(settings.WorkingFolder, structure.Qa);
            var index = _selector.Select(files.Select(Path.GetFileName), "Select file");
            if (index < 0)
            {
                return null;
            }
            return files[index];
        }

        private FileStructure selectStructure(Settings settings)
        {
            var index = _selector.Select(settings.FileStructures.Select(x => x.Qa.Name), "Select structure");
            if (index < 0)
            {
                return null;
            }
            return settings.FileStructures[index];
        }
    }
}
