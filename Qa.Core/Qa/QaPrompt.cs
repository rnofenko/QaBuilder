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
        private readonly IExporter _excelExporter;
        private readonly Comparer _comparer;
        private readonly BinCombiner _binCombiner;
        private readonly Sorter _sorter;
        private readonly Translator _translator;
        private readonly Invertor _invertor;
        private readonly QaFileFinder _qaFileFinder;
        private readonly FourMonthsFileRebuilder _fourMonthsFileRebuilder;

        public QaPrompt(IExporter exporter)
        {
            _qaFileFinder = new QaFileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
            _sorter = new Sorter();
            _translator = new Translator();
            _binCombiner = new BinCombiner();
            _invertor = new Invertor();
            _fourMonthsFileRebuilder = new FourMonthsFileRebuilder();
        }

        public void Start(Settings settings)
        {
            foreach (var structure in settings.FileStructures)
            {
                var qaFiles = _qaFileFinder.Find(settings.WorkingFolder, structure.Qa);

                var files = qaFiles
                    .Select(x => new QaFileParser(settings).Parse(x, structure.Qa))
                    .ToList();

                if (files.IsEmpty())
                {
                    Lo.Wl("No files were detected as QA report for structure=" + structure.Qa.Name, ConsoleColor.Yellow);
                }
                else
                {
                    files = _invertor.Invert(files);
                    files = _binCombiner.Combine(files);
                    files = _translator.Translate(files);
                    var result = _comparer.Compare(files, structure.Qa.CompareFilesMethod);
                    _fourMonthsFileRebuilder.Rebuild(result);
                    result = _sorter.Sort(result);
                    _excelExporter.AddData(structure.Qa.Name, result, settings);
                }
            }

            _excelExporter.Export();
            Lo.Wl().Wl("Comparing was finished.", ConsoleColor.Green);
        }
    }
}