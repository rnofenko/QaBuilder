using System;
using System.Linq;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Parsers;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.Core.Transforms;
using Qa.Core.Translates;

namespace Qa.Core.Qa
{
    public class QaPrompt
    {
        private readonly Settings _settings;
        private readonly IExporter _excelExporter;
        private readonly Comparer _comparer;
        private readonly BinCombiner _binCombiner;
        private readonly Sorter _sorter;
        private readonly Translator _translator;
        private readonly Invertor _invertor;
        private readonly QaFileFinder _qaFileFinder;

        public QaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _qaFileFinder = new QaFileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
            _sorter = new Sorter();
            _translator = new Translator();
            _binCombiner = new BinCombiner();
            _invertor = new Invertor();
        }

        public void Start()
        {
            var qaFiles = _qaFileFinder.Find(_settings);

            var files = qaFiles
                .Select(x => new FileParser(_settings).Parse(x))
                .ToList();
            
            if (files.IsEmpty())
            {
                Lo.Wl("No files were detected as QA report", ConsoleColor.Red);
            }
            else
            {
                files = _invertor.Invert(files);
                files = _binCombiner.Combine(files);
                files = _translator.Translate(files);
                var result = _comparer.Compare(files);
                result = _sorter.Sort(result);
                _excelExporter.Export(result, _settings);
                Lo.Wl().Wl("Comparing was finished.", ConsoleColor.Green);
            }
        }
    }
}