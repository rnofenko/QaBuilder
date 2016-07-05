using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.Parsers.FileReaders;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Parsers
{
    public class QaFileParser
    {
        private readonly int _rowsLimit;

        public QaFileParser(Settings settings)
        {
            _rowsLimit = settings.FileParserRowsLimit;
            if (_rowsLimit == 0)
            {
                _rowsLimit = int.MaxValue;
            }
        }

        public ParsedBatch Parse(string filepath, QaStructure structure, string splitBy)
        {
            Lo.Wl(string.Format("File: {0}", Path.GetFileNameWithoutExtension(filepath)), ConsoleColor.Cyan);
            var splitByIndex = structure.SourceFields.FindIndex(x => x.Name == splitBy);

            var parsers = new Dictionary<string, ValueParser>();
            using (var reader = FileReaderFactory.Create(filepath, structure.GetLineParser()))
            {
                reader.Skip(structure.RowsInHeader + structure.SkipRows);
                string[] parts;
                while ((parts = reader.ParseNextRow()) != null)
                {
                    var splitValue = parts[splitByIndex];
                    if (!parsers.ContainsKey(splitValue))
                    {
                        parsers.Add(splitValue, new ValueParser(structure));
                    }
                    parsers[splitValue].Parse(parts);
                }
            }

            return new ParsedBatch
            {
                Path = filepath,
                Files = parsers.Select(
                    x => new ParsedFile
                    {
                        Fields = x.Value.GetResultFields(),
                        Path = filepath,
                        SplitValue = x.Key
                    })
                    .ToList()
            };
        }

        public ParsedFile Parse(string filepath, QaStructure structure)
        {
            Lo.W("Parse file: ", ConsoleColor.Cyan).Highlight(Path.GetFileNameWithoutExtension(filepath));
            Lo.Watch.Start();
            
            using (var valueParser = new ValueParser(structure))
            {
                using (var reader = FileReaderFactory.Create(filepath, structure.GetLineParser()))
                {
                    try
                    {
                        reader.Skip(structure.RowsInHeader + structure.SkipRows);
                        string[] parts;
                        while ((parts = reader.ParseNextRow()) != null)
                        {
                            if (valueParser.RowsCount > _rowsLimit)
                            {
                                break;
                            }
                            Lo.ShowFileProcessingProgress(valueParser.RowsCount);
                            valueParser.Parse(parts);
                        }
                    }
                    catch (Exception e)
                    {
                        showError(reader, valueParser, e);
                    }
                }

                Lo.Wl().Wl(string.Format("Processed {0,5}k", valueParser.RowsCount / 1000));
                return new ParsedFile {Fields = valueParser.GetResultFields(), Path = filepath };
            }
        }

        private void showError(IFileReader reader, ValueParser valueParser, Exception e)
        {
            var lastLine = reader.GetLastLine();

            Lo.Wl()
                .Wl(string.Format("Error in row {0}.", valueParser.RowsCount), ConsoleColor.Red)
                .Wl(e.Message, ConsoleColor.Red)
                .Wl(e.StackTrace)
                .W("Last line is:");
            if (lastLine == null || lastLine.Length < 500)
            {
                Lo.Wl(lastLine);
            }
            else
            {
                Lo.Wl("Length is " + lastLine);
            }

            Console.ReadLine();
        }
    }
}