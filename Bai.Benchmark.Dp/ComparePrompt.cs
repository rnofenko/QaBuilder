using System;
using Qa.Bai.Benchmark.Dp.Excel;
using Qa.Core.Collectors;
using Qa.Core.Compare;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Bai.Benchmark.Dp
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

            var rawReports = new RawDataCollector().CollectReports(files, _settings.FileStructures);

            var result = _comparer.Compare(rawReports);
            _excelExporter.Export(result, _settings);

            Console.ForegroundColor = ConsoleColor.Green;
            Lo.Wl().Wl("Comparing was finished.");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
