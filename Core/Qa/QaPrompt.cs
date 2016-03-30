using System;
using Qa.Core.Collectors;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.Core.Transforms;

namespace Qa.Core.Qa
{
    public class QaPrompt
    {
        private readonly Settings _settings;
        private readonly FileFinder _fileFinder;
        private readonly IExporter _excelExporter;
        private readonly Comparer _comparer;
        private BinCombiner _binCombiner;

        public QaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _fileFinder = new FileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
            _binCombiner = new BinCombiner();
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
            _binCombiner.Combine(rawReports);

            var result = _comparer.Compare(rawReports);
            _excelExporter.Export(result, _settings);

            Console.ForegroundColor = ConsoleColor.Green;
            Lo.Wl().Wl("Comparing was finished.");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}