using System;
using Qa.Bai.Sbb.Collectors;
using Qa.Bai.Sbb.Excel;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.System;

namespace Qa.Bai.Sbb.Compare
{
    public class ComparePrompt
    {
        private readonly CompareSettings _settings;
        private readonly FileFinder _fileFinder;
        private readonly Exporter _excelExporter;
        private readonly Comparer _comparer;

        public ComparePrompt(Settings settings)
        {
            _settings = new CompareSettings(settings);
            _fileFinder = new FileFinder();
            _excelExporter = new Exporter();
            _comparer = new Comparer();
        }

        public void Start()
        {
            while (true)
            {
                Lo.NewPage("QA Reports");
                showSettings(_settings);

                Lo.Wl()
                    .Wl("Select command:")
                    .Wl("1. Create QA Report");

                var key = Console.ReadKey().KeyChar;
                if (key == '1')
                {
                    doReport();
                    break;
                }
            }
        }

        private void doReport()
        {
            var files = _fileFinder.Find(_settings.WorkingFolder, _settings.FileMask);
            Lo.Wl().Wl($"Found {files.Count} files:");

            var rawReports = new RawDataCollector().CollectReports(files, new CollectionSettings
            {
                FileStructures = _settings.FileStructures,
                ShowError = _settings.ShowNotParsedFiles
            });
            
            var result = _comparer.Compare(rawReports);
            _excelExporter.Export(result, _settings);

            Lo.Wl().Wl("Comparing was finished.");
            Console.ReadKey();
        }

        private void showSettings(CompareSettings settings)
        {
            Lo.Wl().Wl("Current Settings:")
                .Wl($"File mask      = {settings.FileMask}")
                .Wl($"Working folder = {settings.WorkingFolder}");
        }
    }
}
