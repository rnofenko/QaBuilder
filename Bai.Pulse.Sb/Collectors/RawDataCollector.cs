using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.Sbpm.Collectors;

namespace Qa.Bai.Pulse.Sb.Collectors
{
    public class RawDataCollector
    {
        private readonly StructureDetector _structureDetector;
        
        public RawDataCollector()
        {
            _structureDetector = new StructureDetector();
        }

        public List<PulseRawReport> Collect(IEnumerable<string> files, CollectionSettings settings)
        {
            var statisticsByFiles = files
                .Select(x => new RawDataCollector().collect(x, settings))
                .Where(x => x != null)
                .OrderBy(x => x.Structure.Name)
                .ToList();
            return statisticsByFiles;
        }

        private PulseRawReport collect(string filepath, CollectionSettings settings)
        {
            var structure = _structureDetector.Detect(filepath, settings.FileStructures);
            if (structure == null)
            {
                return null;
            }

            var report = new PulseRawReport(structure.Fields)
            {
                Path = filepath,
                Structure = structure
            };
            
            using (var parserGroup = new ValueParserGroup(structure))
            {
                using (var stream = new StreamReader(filepath))
                {
                    for (var i = 0; i < report.Structure.RowsInHeader; i++)
                    {
                        stream.ReadLine();
                    }

                    string line;
                    while ((line = stream.ReadLine()) != null)
                    {
                        parserGroup.Parse(line);                        
                        if ((parserGroup.RowsCount % 25000) == 0)
                        {
                            Lo.Wl(string.Format("Processed {0}", parserGroup.RowsCount));
                        }
                    }
                }
                report.SubReports = parserGroup.GetSubReports();
            }
            return report;
        }
    }
}