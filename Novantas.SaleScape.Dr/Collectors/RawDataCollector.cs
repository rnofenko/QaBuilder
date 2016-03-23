using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.Collectors;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Novantas.SaleScape.Dr.Collectors
{
    public class RawDataCollector
    {
        private readonly StructureDetector _structureDetector;
        
        public RawDataCollector()
        {
            _structureDetector = new StructureDetector();
        }

        public List<RawReport> CollectReports(IEnumerable<string> files, CollectionSettings settings)
        {
            var statisticsByFiles = files
                .Select(x => new RawDataCollector().collectReport(x, settings))
                .Where(x => x != null)
                .OrderBy(x => x.Structure.Name)
                .ToList();
            return statisticsByFiles;
        }

        private RawReport collectReport(string filepath, CollectionSettings settings)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Lo.Wl($"File: {Path.GetFileNameWithoutExtension(filepath)}");
            Console.ResetColor();
            var detected = _structureDetector.Detect(filepath, settings.FileStructures);
            if (detected == null)
            {
                return null;
            }

            var report = new RawReport
            {
                Path = filepath,
                Structure = detected.Structure,
                FieldsCount = detected.FieldsCount
            };
            
            using (var valueParser = new ValueParser(detected.Structure.Fields))
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
                        var parts = line.Split(new[] {report.Structure.Delimeter }, StringSplitOptions.None);
                        valueParser.Parse(parts);
                        if ((valueParser.RowsCount%1000000) == 0)
                        {
                            Lo.Wl($"Processed {valueParser.RowsCount}");
                        }
                    }
                }
                report.Fields = valueParser.GetResultFields();
                report.RowsCount = valueParser.RowsCount;
            }
            return report;
        }
    }
}