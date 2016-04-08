using System;
using System.Diagnostics;
using System.IO;
using Qa.Core.System;

namespace Qa.Core.Collectors
{
    public class RawDataCollector
    {
        private static Stopwatch _watch;
        private static Stopwatch _watch2;
        private static Stopwatch _watch3;

        public RawReport Collect(RawReport report)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Lo.Wl($"File: {Path.GetFileNameWithoutExtension(report.Path)}");
            Console.ResetColor();
            var structure = report.Structure;
            var lineParser = structure.GetLineParser();

            if (_watch == null)
            {
                _watch = Stopwatch.StartNew();
                _watch2 = new Stopwatch();
                _watch3 = new Stopwatch();
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
                        _watch2.Start();
                        var parts = lineParser.Parse(line);
                        _watch2.Stop();
                        _watch3.Start();
                        valueParser.Parse(parts);
                        _watch3.Stop();
                        if ((valueParser.RowsCount % 250000) == 0)
                        {
                            Lo.Wl($"Processed {valueParser.RowsCount / 1000,5}k  Time:{_watch.Elapsed}");
                        }
                    }
                }

                report.Fields = valueParser.GetResultFields();
                return report;
            }
        }
    }
}