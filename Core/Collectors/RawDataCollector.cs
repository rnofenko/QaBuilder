using System;
using System.Diagnostics;
using System.IO;
using Qa.Core.System;

namespace Qa.Core.Collectors
{
    public class RawDataCollector
    {
        private static Stopwatch _watch;

        public RawReport Collect(RawReport report)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Lo.Wl(string.Format("File: {0}", Path.GetFileNameWithoutExtension(report.Path)));
            Console.ResetColor();
            var structure = report.Structure;
            var lineParser = structure.GetLineParser();

            if (_watch == null)
            {
                _watch = Stopwatch.StartNew();
            }

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
                        if ((valueParser.RowsCount % 250000) == 0)
                        {
                            Lo.Wl(string.Format("Processed {0,5}k  Time:{1}", valueParser.RowsCount/1000, _watch.Elapsed));
                        }
                    }
                }

                report.Fields = valueParser.GetResultFields();
                return report;
            }
        }
    }
}