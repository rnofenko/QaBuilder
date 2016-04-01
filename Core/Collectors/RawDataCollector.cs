using System;
using System.IO;
using Qa.Core.System;

namespace Qa.Core.Collectors
{
    public class RawDataCollector
    {
        public RawReport Collect(RawReport report)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Lo.Wl($"File: {Path.GetFileNameWithoutExtension(report.Path)}");
            Console.ResetColor();
            var structure = report.Structure;
            var lineParser = structure.GetLineParser();
            using (var valueParser = new ValueParser(structure.Fields))
            {
                using (var stream = new StreamReader(report.Path))
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
                        if ((valueParser.RowsCount % 500000) == 0)
                        {
                            Lo.Wl($"Processed {valueParser.RowsCount}");
                        }
                    }
                }

                return report;
            }
        }
    }
}