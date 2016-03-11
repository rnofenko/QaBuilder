using System;
using Qa.Bai.Pulse.Sb.Collectors;
using Qa.Bai.Pulse.Sb.Excel;
using Qa.Bai.Pulse.Sb.Transforms;
using Qa.Bai.Sbp.Compare;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.Sbpm.Collectors;
using Qa.Sbpm.Compare;
using Qa.System;

namespace Qa.Bai.Pulse.Sb.Compare
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
            Lo.NewPage("QA Reports");
            doReport();
        }

        private void doReport()
        {
            var files = _fileFinder.Find(_settings.WorkingFolder, _settings.FileMask);
            Lo.Wl().Wl($"Found {files.Count} files:");

            var rawReports = new RawDataCollector().Collect(files, new CollectionSettings
            {
                FileStructures = _settings.FileStructures,
                ShowError = _settings.ShowNotParsedFiles
            });

            new SbpReportTransformer().Transform(rawReports);
            
            var result = _comparer.Compare(rawReports);
            _excelExporter.Export(result, _settings);

            Lo.Wl().Wl("Comparing was finished.");
            Console.ReadKey();
        }
    }
}
