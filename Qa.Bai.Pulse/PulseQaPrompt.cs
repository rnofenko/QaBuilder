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
            var structure = _settings.FileStructures.First().Qa;
            var batches = _qaFileFinder
                .Find(_settings.WorkingFolder, structure)
                .Select(x => new QaFileParser(_settings).Parse(x, structure, "STATE"))
                .ToList();
            alignFiles(batches, structure);

            if (batches.IsEmpty())
            {
                Lo.Wl("No files were detected as QA report for structure=" + structure.Name, ConsoleColor.Yellow);
            }
            else
            {
                var result = _comparer.Compare(batches, structure.CompareFilesMethod);
                _excelExporter.AddData(structure.Name, result.First(x => x.SplitValue == PulseConsts.NATIONAL), _settings);
                foreach (var packet in result.Where(x => x.SplitValue != PulseConsts.NATIONAL))
                {
                    _excelExporter.AddData(structure.Name, packet, _settings);
                }
            }
            
            _excelExporter.Export();
            Lo.Wl().Wl("Comparing was finished.", ConsoleColor.Green);
        }

        private void alignFiles(List<ParsedBatch> batches, QaStructure structure)
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
                        batch.CreateIfAbsent(parsedFile.SplitValue, structure);
                    }
                }
            }
        }
    }
}
