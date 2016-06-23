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
        private FourMonthsFileRebuilder _fourMonthsFileRebuilder;

        public PulseQaPrompt(Settings settings, IExporter exporter)
        {
            _settings = settings;
            _qaFileFinder = new QaFileFinder();
            _excelExporter = exporter;
            _comparer = new Comparer();
            _fourMonthsFileRebuilder = new FourMonthsFileRebuilder();
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

                var nationalPacket = result.First(x => x.SplitValue == PulseConsts.NATIONAL);
                _fourMonthsFileRebuilder.Rebuild(nationalPacket);
                _excelExporter.AddData(structure.Name, nationalPacket, _settings);
                foreach (var packet in result.Where(x => x.SplitValue != PulseConsts.NATIONAL))
                {
                    _fourMonthsFileRebuilder.Rebuild(packet);
                    _excelExporter.AddData(structure.Name, packet, _settings);
                }
            }
            
            _excelExporter.Export();
            Lo.Wl().Wl("Comparing was finished.", ConsoleColor.Green);
        }

        private void alignFiles(List<ParsedBatch> batches, QaStructure structure)
        {
            foreach (var parsedBatch in batches.Where(x=>x!=null))
            {
                parsedBatch.Sort();
            }

            foreach (var parsedBatch in batches.Where(x => x != null))
            {
                foreach (var parsedFile in parsedBatch.Files)
                {
                    foreach (var batch in batches.Where(x => x != null))
                    {
                        batch.CreateIfAbsent(parsedFile.SplitValue, structure);
                    }
                }
            }
        }
    }
}
