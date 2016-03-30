using Qa.Bai.Pulse.Sb.Collectors;
using Qa.Bai.Pulse.Sb.Excel;
using Qa.Bai.Pulse.Sb.Transforms;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.Sbpm.Collectors;

namespace Qa.Bai.Pulse.Sb.Compare
{
    public class PulseComparePrompt
    {
        private readonly CompareSettings _settings;
        private readonly FileFinder _fileFinder;
        private readonly Exporter _excelExporter;
        private readonly PulseComparer _pulseComparer;

        public PulseComparePrompt(Settings settings)
        {
            _settings = new CompareSettings(settings);
            _fileFinder = new FileFinder();
            _excelExporter = new Exporter();
            _pulseComparer = new PulseComparer();
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
            
            var result = _pulseComparer.Compare(rawReports);
            _excelExporter.Export(result, _settings);

            Lo.Wl().Wl("Comparing was finished.");
        }
    }
}
