using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Qa.Core.Parsers.FileReaders;
using Qa.Core.System;

namespace Qa.Core.Parsers
{
    public class FileParser
    {
        private static Stopwatch _watch;
        
        public ParsedBatch Parse(FileParseArgs args, string splitBy)
        {
            Lo.Wl(string.Format("File: {0}", Path.GetFileNameWithoutExtension(args.Path)), ConsoleColor.Cyan);
            var splitByIndex = args.Structure.SourceFields.FindIndex(x => x.Name == splitBy);

            var parsers = new Dictionary<string, ValueParser>();
            using (var reader = FileReaderFactory.Create(args.Path, args.Structure))
            {
                reader.Skip(args.Structure.RowsInHeader);
                string[] parts;
                while ((parts = reader.ReadRow()) != null)
                {
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
            Lo.W("Parse file: ", ConsoleColor.Cyan).Wl(Path.GetFileNameWithoutExtension(args.Path), ConsoleColor.Green);
            if (_watch == null)
            {
                _watch = Stopwatch.StartNew();
            }

            using (var valueParser = new ValueParser(args.Structure))
            {
                using (var reader = FileReaderFactory.Create(args.Path, args.Structure))
                {
                    reader.Skip(args.Structure.RowsInHeader);
                    string[] parts;
                    while ((parts = reader.ReadRow()) != null)
                    {
                        if ((valueParser.RowsCount % 20000) == 0)
                        {
                            if (valueParser.RowsCount == 20000)
                            {
                                //break;
                            }

                            if ((valueParser.RowsCount % 1000000) == 0)
                            {
                                Lo.Wl().W(string.Format("Processed {0,2}m Time:{1:mm:ss.fff} ", valueParser.RowsCount / 1000000, new DateTime().AddTicks(_watch.ElapsedTicks)));
                            }
                            else
                            {
                                Lo.W(".");
                            }
                        }
                        valueParser.Parse(parts);
                    }
                }

                Lo.Wl().Wl(string.Format("Processed {0,5}k", valueParser.RowsCount / 1000));
                return new ParsedFile {Fields = valueParser.GetResultFields(), Structure = args.Structure, Path = args.Path};
            }
        }
    }
}