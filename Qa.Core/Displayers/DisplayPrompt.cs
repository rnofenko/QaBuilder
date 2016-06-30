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

        public DisplayPrompt()
        {
            _fileFinder = new QaFileFinder();
            _selector = new SelectorPrompt();
            _displayer = new Displayer();
        }

        public void Start(Settings settings)
        {
            Lo.Wl().Wl().Wl(string.Format("..................... Displayer - {0} .........................", settings.Project))
                .Wl(string.Format("Current folder is {0}", settings.WorkingFolder));
            var selectedStructure = selectStructure(settings);
            var selectedFile = selectFile(settings, selectedStructure);
            if (selectedFile == null)
            {
                return;
            }
            
            while (true)
            {
                Lo.Wl()
                    .Wl("Select command:")
                    .Wl("1. Show one column")
                    .Wl("2. Show All columns");
                
                var key = Console.ReadKey();
                if (key.KeyChar == '1')
                {
                    var column = _selector.Select(selectedStructure.Fields.Select(x => x.Name), "Select field");
                    if (column >= 0)
                    {
                        _displayer.DisplayColumn(selectedFile, selectedStructure, column);
                    }
                }
                else if (key.KeyChar == '2')
                {
                    _displayer.DisplayByRows(selectedFile, selectedStructure);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

        private string selectFile(Settings settings, FileStructure structure)
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
