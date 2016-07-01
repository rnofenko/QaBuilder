using System;
using System.IO;
using System.Linq;
using Qa.Core.Selectors;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Displayers
{
    public class DisplayPrompt
    {
        private readonly QaFileFinder _fileFinder;
        private readonly SelectorPrompt _selector;
        private readonly Displayer _displayer;
        private readonly FileSelector _fileSelector;
        private readonly StructureSelector _structureSelector;

        public DisplayPrompt()
        {
            _fileFinder = new QaFileFinder();
            _selector = new SelectorPrompt();
            _displayer = new Displayer();
            _fileSelector = new FileSelector();
            _structureSelector = new StructureSelector();
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
            var selectedStructure = _structureSelector.Select(settings);
            var file = _fileSelector.SelectAnyFile(settings.WorkingFolder);
            if (file == null || selectedStructure == null)
            {
                return;
            }
            
            _displayer.SplitRowsByFields(file, selectedStructure);
        }

        private void showOneColumn(Settings settings)
        {
            var selectedStructure = _structureSelector.Select(settings);
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
            var selectedStructure = _structureSelector.Select(settings);
            var selectedFile = _fileSelector.SelectAnyFile(settings.WorkingFolder);
            if (selectedFile == null || selectedStructure == null)
            {
                return;
            }
            _displayer.DisplayWholeRows(selectedFile, selectedStructure);
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
    }
}
