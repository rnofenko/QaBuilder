using System;
using System.Linq;
using Q2.Core.Collectors;
using Q2.Core.Compare;
using Q2.Core.Excel;
using Q2.Core.Extensions;
using Q2.Core.Structure;
using Q2.Core.System;
using Q2.Core.Transforms;

namespace Q2.Core.Qa
{
    public class QaPrompt
    {
        private readonly Settings _settings;
        private readonly FileFinder _fileFinder;
        private readonly IExporter _excelExporter;
        private readonly Comparer _comparer;
        private readonly BinCombiner _binCombiner;
        private readonly StructureDetector _detector;

        public QaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _fileFinder = new FileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
            _binCombiner = new BinCombiner();
            _detector = new StructureDetector();
        }

        public void Start()
        {
            Lo.NewPage("QA Reports");
            doReport();
        }

        private void doReport()
        {
            var rawReports = _fileFinder
                .Find(_settings.WorkingFolder, "*.*")
                .Select(createRawReport)
                .Where(x => x != null)
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

        private RawReport createRawReport(string path)
        {
            var structure = _detector.Detect(path, _settings.FileStructures.Select(x => x.Qa));
            if (structure == null)
            {
                return null;
            }
            return new RawReport {Path = path, Structure = structure};
        }
    }
}