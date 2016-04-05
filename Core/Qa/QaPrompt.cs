using System;
using System.Linq;
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
        private readonly BinCombiner _binCombiner;

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
            var detector = new StructureDetector();
            var rawReports = _fileFinder
                .Find(_settings.WorkingFolder, _settings.FileMask)
                .Select(x => new RawReport {Path = x, Structure = detector.Detect(x, _settings.FileStructures)})
                .Where(x => x.Structure != null)
                .Select(x => new RawDataCollector().Collect(x))
                .ToList();

            if (rawReports.IsEmpty())
            {
                Lo.Wl("No files were detected as QA report", ConsoleColor.Red);
            }
            else
            {
                _binCombiner.Combine(rawReports);
                var result = _comparer.Compare(rawReports);
                _excelExporter.Export(result, _settings);

                Lo.Wl().Wl("Comparing was finished.", ConsoleColor.Green);
            }
            Console.ReadKey();
        }
    }
}