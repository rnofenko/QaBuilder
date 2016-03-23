using System;
using Qa.Core.Collectors;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Novantas.SaleScape.Dr
{
    public class ComparePrompt
    {
        private readonly IExporter _exporter;
        private readonly CompareSettings _compareSettings;
        private readonly FileFinder _fileFinder;
        private readonly Comparer _comparer;
        private readonly Settings _settings;

        public ComparePrompt(Settings settings, IExporter exporter)
        {
            _exporter = exporter;
            _settings = settings;
            _compareSettings = new CompareSettings(settings);
            _fileFinder = new FileFinder();
            _comparer = new Comparer();
        }

        public void Start()
        {
            Lo.NewPage("QA Reports");
            doReport();
        }

        private void doReport()
        {
            var files = _fileFinder.Find(_compareSettings.WorkingFolder, _compareSettings.FileMask);
            Lo.Wl().Wl($"Found {files.Count} files:");

            var rawReports = new RawDataCollector().CollectReports(files, _compareSettings.FileStructures);

            var result = _comparer.Compare(rawReports);
            _exporter.Export(result, _settings);

            Console.ForegroundColor = ConsoleColor.Green;
            Lo.Wl().Wl("Comparing was finished.");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
