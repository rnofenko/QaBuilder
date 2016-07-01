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
        private readonly PathFinder _pathFinder;

        public FormatPrompt()
        {
            _formatter = new Formatter();
            _structureDetector = new StructureDetector();
            _pathFinder = new PathFinder();
        }

        public void Start(Settings settings)
        {
            showSettings(settings);
            foreach (var fileStructure in settings.FileStructures)
            {
                format(fileStructure.Format, settings);
            }
        }

        private void format(FormatStructure structure, Settings settings)
        {
            var files = _pathFinder.Find(settings.WorkingFolder, structure.FileMask).ToList();
            Lo.Wl().Wl(string.Format("Found {0} files for {1}:", files.Count, structure.Name));

            var ask = AskFileResult.None;
            foreach (var filepath in files.Where(x => _structureDetector.Match(x, structure)))
            {
                var file = new FormattingFile
                {
                    SourcePath = filepath,
                    FormatStructure = structure,
                    DestinationPath = Path.Combine(settings.WorkingFolder, Path.GetFileNameWithoutExtension(filepath) + "." + DESTINATION_FILE_EXTENSION)
                };
                if (ask != AskFileResult.YesForAll)
                {
                    ask = askFormat(file);
                }
                if (ask == AskFileResult.Yes || ask == AskFileResult.YesForAll)
                {
                    _formatter.Format(file);
                }
                else if (ask == AskFileResult.NoForAll)
                {
                    break;
                }
            }

            Lo.Wl().Wl("Formatting was finished.");
            Console.ReadKey();
        }
        
        private void showSettings(Settings settings)
        {
            Lo.Wl(1)
                .Wl("Formatting files")
                .Wl("Source File Settings:")
                .Wl(string.Format("Working folder        = {0}", settings.WorkingFolder))
                .Wl(string.Format("File mask             = {0}", SOURCE_FILE_MASK))
                .Wl()
                .Wl("Destination File Settings:")
                .Wl(string.Format("New File Extension    = {0}", DESTINATION_FILE_EXTENSION));
        }

        private AskFileResult askFormat(FormattingFile file)
        {
            var yesNoPrompt = "Y(Yes) / N(No) / A(Yes for all) / ESC(No for all): ";

            Lo.Wl()
                .Wl("Are sure that you want to format file")
                .Wl(string.Format("  from {0} to {1}", Path.GetFileName(file.SourcePath), Path.GetFileName(file.DestinationPath)))
                .W(yesNoPrompt);
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    return AskFileResult.NoForAll;
                }
                var keyChar = key.KeyChar.ToString().ToLower();
                if (keyChar == "y")
                {
                    return AskFileResult.Yes;
                }
                if (keyChar == "a")
                {
                    return AskFileResult.YesForAll;
                }
                if (keyChar == "n")
                {
                    return AskFileResult.No;
                }
                Lo.Wl().W(yesNoPrompt);
            }
        }
    }
}
