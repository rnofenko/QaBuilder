using System;
using System.IO;
using System.Linq;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.System;

namespace Qa.Core.Format
{
    public class FormatPrompt
    {
        private readonly StructureDetector _structureDetector;
        private readonly FormatSettings _settings;
        private readonly Formatter _formatter;
        private readonly FileFinder _fileFinder;

        public FormatPrompt(Settings settings)
        {
            _settings = new FormatSettings(settings);
            _formatter = new Formatter(_settings);
            _structureDetector = new StructureDetector();
            _fileFinder = new FileFinder();
        }

        public void Start()
        {
            Lo.NewPage("Formatting files");
            showSettings(_settings);

            var files = _fileFinder.Find(_settings.WorkingFolder, _settings.SourceFileMask).ToList();
            Lo.Wl().Wl($"Found {files.Count} files:");

            var detectSettings = new StructureDetectSettings { Delimeter = _settings.SourceDelimeter, FileStructures = _settings.FileStructures };
            foreach (var filepath in files)
            {
                var detected = _structureDetector.Detect(filepath, detectSettings);
                if (detected.Error.IsNotEmpty())
                {
                    Lo.Wl()
                        .Wl(detected.FilePath)
                        .Wl("ERROR      : " + detected.Error)
                        .Wl($"Structure  : {detected.Structure?.Name}", detected.Structure != null)
                        .Wl($"FieldsCount: {detected.FieldsCount}");
                }
                else
                {
                    askFormat(new FormattingFile {SourcePath = filepath, Structure = detected.Structure});
                }
            }

            Lo.Wl().Wl("Formatting was finished.");
            Console.ReadKey();
        }
        
        private void showSettings(FormatSettings settings)
        {
            Lo.Wl()
                .Wl("Source File Settings:")
                .Wl($"Working folder        = {settings.WorkingFolder}")
                .Wl($"Skip Rows Count       = {settings.SkipRows}")
                .Wl($"File mask             = {settings.SourceFileMask}")
                .Wl($"Delimeter             = {settings.SourceDelimeter}")
                .Wl()
                .Wl("Destination File Settings:")
                .Wl($"New File Extension    = {settings.DestinationFileExtension}")
                .Wl($"New Delimeter         = {settings.DestinationDelimeter}");
        }

        private void askFormat(FormattingFile file)
        {
            file.DestinationPath = Path.Combine(_settings.WorkingFolder, Path.GetFileNameWithoutExtension(file.SourcePath) + "." + _settings.DestinationFileExtension);
            Lo.Wl()
                .Wl("Are sure that you want to format file")
                .Wl($"  from   {file.SourcePath}")
                .Wl($"  to     {file.DestinationPath}")
                .Wl("Y(Yes) / Any key(No)");
            var key = Console.ReadKey();
            if (key.KeyChar.ToString().ToLower() == "y")
            {
                _formatter.Format(file);
            }
        }
    }
}
