using System;
using System.IO;
using System.Linq;
using Qa.Core.Parsers.FileReaders;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Editors
{
    public class Editor
    {
        public void AddColumn(string sourceFile, IStructure structure, int position, string defaultValue)
        {
            processFile(sourceFile, structure, row =>
            {
                var pos = position > row.Length ? row.Length : position;
                var list = row.ToList();
                list.Insert(pos, defaultValue);
                return string.Join(structure.Delimiter, list);
            });
        }

        public void DeleteColumn(string sourceFile, IStructure structure, int position)
        {
            processFile(sourceFile, structure, row =>
            {
                var pos = position > row.Length ? row.Length : position;
                var list = row.ToList();
                list.RemoveAt(pos);
                return string.Join(structure.Delimiter, list);
            });
        }

        private void processFile(string sourceFile, IStructure structure, Func<string[], string> rowAction)
        {
            using (var reader = FileReaderFactory.Create(sourceFile, structure.GetLineParser()))
            {
                using (var writer = new StreamWriter(sourceFile + "." + DateTime.Now.Ticks))
                {
                    for (var i = 0; i < structure.SkipRows; i++)
                    {
                        writer.WriteLine(reader.ReadNextRow());
                    }
                    
                    while (true)
                    {
                        var row = reader.ParseNextRow();
                        if (row.IsEmpty())
                        {
                            break;
                        }

                        var rebuiltRow = rowAction(row);
                        writer.WriteLine(rebuiltRow);
                        Lo.ShowFileProcessingProgress(reader.RowNumber);
                    }
                }
                Lo.ShowFileProcessingProgress(reader.RowNumber);
            }
        }
    }
}