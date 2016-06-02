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
        private readonly FileFinder _fileFinder;
        private readonly IExporter _excelExporter;
        private readonly Comparer _comparer;
        private readonly BinCombiner _binCombiner;
        private readonly StructureDetector _detector;
        private readonly Sorter _sorter;
        private readonly Translator _translator;
        private readonly Invertor _invertor;

        public QaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _fileFinder = new FileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
            _sorter = new Sorter();
            _translator = new Translator();
            _binCombiner = new BinCombiner();
            _detector = new StructureDetector();
            _invertor = new Invertor();
        }

        public void Start()
        {
            var files = _fileFinder
                .Find(_settings.WorkingFolder)
                .Select(getParseArgs)
                .Where(x => x != null)
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

        private FileParseArgs getParseArgs(string path)
        {
            var structure = _detector.Detect(path, _settings.FileStructures.Select(x => x.Qa));
            if (structure == null)
            {
                return null;
            }
            return new FileParseArgs { Path = path, Structure = structure};
        }
    }
}