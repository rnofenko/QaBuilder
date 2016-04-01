using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Collectors
{
    public class RawDataCollector
    {
        private readonly StructureDetector _structureDetector;
        
        public RawDataCollector()
        {
            _structureDetector = new StructureDetector();
        }

        public List<RawReport> CollectReports(IEnumerable<string> files, List<FileStructure> structures)
        {
            var statisticsByFiles = files
                .Select(x => new RawDataCollector().collectReport(x, structures))
                .Where(x => x != null)
                .OrderBy(x => x.Structure.Name)
                .ToList();
            return statisticsByFiles;
        }

        private RawReport collectReport(string filepath, List<FileStructure> structures)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Lo.Wl($"File: {Path.GetFileNameWithoutExtension(filepath)}");
            Console.ResetColor();
            var detected = _structureDetector.Detect(filepath, structures);
            if (detected == null)
            {
                return null;
            }

            var structure = detected.Structure;
            var lineParser = structure.GetLineParser();
            
            using (var valueParser = new ValueParser(detected.Structure.Fields))
            {
                using (var stream = new StreamReader(filepath))
                {
                    for (var i = 0; i < structure.RowsInHeader; i++)
                    {
                        stream.ReadLine();
                    }

                    string line;
                    while ((line = stream.ReadLine()) != null)
                    {
                        var parts = lineParser.Parse(line);
                        valueParser.Parse(parts);
                        if ((valueParser.RowsCount%500000) == 0)
                        {
                            Lo.Wl($"Processed {valueParser.RowsCount}");
                        }
                    }
                }

                return new RawReport
                {
                    Path = filepath,
                    Structure = structure,
                    FieldsCount = detected.FieldsCount,
                    Fields = valueParser.GetResultFields(),
                    RowsCount = valueParser.RowsCount
                };
            }
        }
    }
}