using System;
using System.Collections.Generic;
using System.Linq;
using Q2.Core.Structure;
using Qa.Core;
using Qa.Core.Compare;
using Qa.Core.Excel;
using Qa.Core.Parsers;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Bai.Pulse
{
    public class PulseQaPrompt
    {
        private readonly Settings _settings;
        private readonly FileFinder _fileFinder;
        private readonly IExporter _excelExporter;
        private readonly Comparer _comparer;
        private readonly StructureDetector _detector;

        public PulseQaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _fileFinder = new FileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
            _detector = new StructureDetector();
        }

        public void Start()
        {
            Lo.NewPage("QA Reports");
            var batches = _fileFinder
                .Find(_settings.WorkingFolder, "*.*")
                .Select(getParseArgs)
                .Where(x => x != null)
                .Select(x => new FileParser().Parse(x, "STATE"))
                .ToList();
            alignFiles(batches);

            if (batches.IsEmpty())
            {
                Lo.Wl("No files were detected as QA report", ConsoleColor.Red);
            }
            else
            {
                var result = _comparer.Compare(batches);
                _excelExporter.Export(result, _settings);

                Lo.Wl().Wl("Comparing was finished.", ConsoleColor.Green);
            }
            Console.ReadKey();
        }

        private FileParseArgs getParseArgs(string path)
        {
            var structure = _detector.Detect(path, _settings.FileStructures.Select(x => x.Qa));
            if (structure == null)
            {
                return null;
            }
            return new FileParseArgs { Path = path, Structure = structure };
        }

        private void alignFiles(List<ParsedBatch> batches)
        {
            foreach (var parsedBatch in batches)
            {
                parsedBatch.Sort();
            }

            foreach (var parsedBatch in batches)
            {
                foreach (var parsedFile in parsedBatch.Files)
                {
                    foreach (var batch in batches)
                    {
                        batch.CreateIfAbsent(parsedFile.SplitValue);
                    }
                }
            }
        }
    }
}
