using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core;
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

        public List<RawReport> Collect(IEnumerable<string> files, CollectionSettings settings)
        {
            var statisticsByFiles = files
                .Select(x => new RawDataCollector().collect(x, settings))
                .Where(x => x.Error.IsEmpty())
                .OrderBy(x => x.Structure.Name)
                .ToList();
            return statisticsByFiles;
        }

        private RawReport collect(string filepath, CollectionSettings settings)
        {
            var detected = _structureDetector.Detect(filepath, settings.FileStructures);

            var report = new RawReport(detected.Structure.Fields)
            {
                Path = filepath,
                Error = detected.Error,
                Structure = detected.Structure,
                FieldsCount = detected.FieldsCount
            };
            if (report.Error.IsNotEmpty())
            {
                return report;
            }

            using (var parserGroup = new ValueParserGroup(detected.Structure))
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
                            Lo.Wl($"Processed {parserGroup.RowsCount}");
                        }
                    }
                }
                report.SubReports = parserGroup.GetSubReports();
            }            
            print(report, settings);
            return report;
        }
        
        private void print(RawReport stats, CollectionSettings settings)
        {
            if (stats.Error.IsEmpty() || !settings.ShowError)
            {
                return;
            }

            Lo.Wl()
                .Wl(stats.Path)
                .Wl("ERROR      : " + stats.Error, stats.Error.IsNotEmpty())
                .Wl($"Structure  : {stats.Structure?.Name}", stats.Structure != null)
                .Wl($"FieldsCount: {stats.FieldsCount}");
        }
    }
}