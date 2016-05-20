﻿using System;
using System.Linq;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Parsers;
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
        private readonly StructureDetector _detector;
        private Sorter _sorter;

        public QaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _fileFinder = new FileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
            _sorter = new Sorter();
            _binCombiner = new BinCombiner();
            _detector = new StructureDetector();
        }

        public void Start()
        {
            var files = _fileFinder
                .Find(_settings.WorkingFolder)
                .Select(getParseArgs)
                .Where(x => x != null)
                .Select(x => new FileParser().Parse(x))
                .ToList();

            if (files.IsEmpty())
            {
                Lo.Wl("No files were detected as QA report", ConsoleColor.Red);
            }
            else
            {
                _binCombiner.Combine(files);
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