using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Q2.Core.Collectors;
using Q2.Core.Extensions;
using Q2.Core.System;

namespace Q2.Core.Parsers
{
    public class FileParser
    {
        private static Stopwatch _watch;

        public ParsedBatch Parse(FileParseArgs args, string splitBy)
        {
            Lo.Wl(string.Format("File: {0}", Path.GetFileNameWithoutExtension(args.Path)), ConsoleColor.Cyan);
            var lineParser = args.Structure.GetLineParser();
            var splitByIndex = args.Structure.SourceFields.FindIndex(x => x.Name == splitBy);

            var parsers = new Dictionary<string, ValueParser>();
            using (var stream = new StreamReader(args.Path))
            {
                for (var i = 0; i < args.Structure.RowsInHeader; i++)
                {
                    stream.ReadLine();
                }

                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    var parts = lineParser.Parse(line);
                    var splitValue = parts[splitByIndex];
                    if (!parsers.ContainsKey(splitValue))
                    {
                        parsers.Add(splitValue, new ValueParser(args.Structure));
                    }
                    parsers[splitValue].Parse(parts);
                }
            }

            return new ParsedBatch
            {
                Structure = args.Structure,
                Path = args.Path,
                Files = parsers.Select(
                    x => new ParsedFile
                    {
                        Fields = x.Value.GetResultFields(),
                        Structure = args.Structure,
                        Path = args.Path,
                        SplitValue = x.Key
                    })
                    .ToList()
            };
        }

        public ParsedFile Parse(FileParseArgs args)
        {
            Lo.Wl(string.Format("File: {0}", Path.GetFileNameWithoutExtension(args.Path)), ConsoleColor.Cyan);
            var lineParser = args.Structure.GetLineParser();

            if (_watch == null)
            {
                _watch = Stopwatch.StartNew();
            }

            using (var valueParser = new ValueParser(args.Structure))
            {
                using (var stream = new StreamReader(args.Path))
                {
                    for (var i = 0; i < args.Structure.RowsInHeader; i++)
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

                return new ParsedFile {Fields = valueParser.GetResultFields(), Structure = args.Structure, Path = args.Path};
            }
        }
    }
}