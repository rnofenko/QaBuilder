using System;
using Qa.Core.Collectors;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Qa
{
    public class QaPrompt
    {
        private readonly Settings _settings;
        private readonly FileFinder _fileFinder;
        private readonly IExporter _excelExporter;
        private readonly Comparer _comparer;

        public QaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _fileFinder = new FileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
        }

        public void Start()
        {
            Lo.NewPage("QA Reports");
            doReport();
        }

        private void doReport()
        {
            var files = _fileFinder.Find(_settings.WorkingFolder, new CompareSettings(_settings).FileMask);
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