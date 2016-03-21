using System;
using System.IO;
using System.Linq;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Format
{
    public class FormatPrompt
    {
        private const string SOURCE_FILE_MASK = "*.csv";
        private const string DESTINATION_FILE_EXTENSION = "txt";
        private readonly StructureDetector _structureDetector;
        private readonly Formatter _formatter;
        private readonly FileFinder _fileFinder;
        private readonly Settings _settings;

        public FormatPrompt(Settings settings)
        {
            _settings = settings;
            _formatter = new Formatter();
            _structureDetector = new StructureDetector();
            _fileFinder = new FileFinder();
        }

        public void Start()
        {
            Lo.NewPage("Formatting files");
            showSettings();

            var files = _fileFinder.Find(_settings.WorkingFolder, SOURCE_FILE_MASK).ToList();
            Lo.Wl().Wl($"Found {files.Count} files:");
            
            foreach (var filepath in files)
            {
                var detected = _structureDetector.Detect(filepath, _settings.Project.FormatSchemes);
                if (detected != null)
                {
                    askFormat(new FormattingFile {SourcePath = filepath, FormatStructure = detected});
                }
            }

            Lo.Wl().Wl("Formatting was finished.");
            Console.ReadKey();
        }
        
        private void showSettings()
        {
            Lo.Wl()
                .Wl("Source File Settings:")
                .Wl($"Working folder        = {_settings.WorkingFolder}")
                .Wl($"File mask             = {SOURCE_FILE_MASK}")
                .Wl()
                .Wl("Destination File Settings:")
                .Wl($"New File Extension    = {DESTINATION_FILE_EXTENSION}");
        }

        private void askFormat(FormattingFile file)
        {
            file.DestinationPath = Path.Combine(_settings.WorkingFolder, Path.GetFileNameWithoutExtension(file.SourcePath) + "." + DESTINATION_FILE_EXTENSION);
            var fromFileName = Path.GetFileName(file.SourcePath);
            var intoFileName = Path.GetFileName(file.DestinationPath);
            Lo.Wl()
                .Wl("Are sure that you want to format file")
                .Wl($"  from   {fromFileName}")
                .Wl($"  to     {intoFileName}")
                .Wl("Y(Yes) / Any key(No)");
            var key = Console.ReadKey();
            if (key.KeyChar.ToString().ToLower() == "y")
            {
                _formatter.Format(file);
            }
        }
    }
}
