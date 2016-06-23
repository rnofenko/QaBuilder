using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IExporter _excelExporter;
        private readonly Comparer _comparer;
        private readonly QaFileFinder _qaFileFinder;

        public PulseQaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _qaFileFinder = new QaFileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
        }

        public void Start()
        {
            var qaFiles = _qaFileFinder.Find(_settings);
            
            var batches = qaFiles
                .Select(x => new FileParser(_settings).Parse(x, "STATE"))
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
