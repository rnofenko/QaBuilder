using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.Parsers.FileReaders;
using Qa.Core.System;

namespace Qa.Core.Structure
{
    public class StructureDetector
    {
        private readonly FileMaskFilter _fileMaskFilter;

        public StructureDetector()
        {
            _fileMaskFilter = new FileMaskFilter();
        }

        public T Detect<T>(string filepath, IEnumerable<T> sourceStructures) where T : IStructure
        {
            var structures = new List<T>();

            foreach (var structure in sourceStructures.Where(x=> _fileMaskFilter.IsMatch(x.FileMask,filepath)))
            {
                using (var reader = FileReaderFactory.Create(filepath, structure))
                {
                    reader.Skip(structure.SkipRows + structure.RowsInHeader);
                    var fields = reader.ParseNextRow();
                    if (fields.Length == structure.CountOfFieldsInFile)
                    {
                        structures.Add(structure);
                    }
                    else
                    {
                        Lo.Wl("Structure " + structure.Name + " and file " + Path.GetFileName(filepath) + " are incompatible. Fields count = " +
                              fields.Length + " structure's fields count=" + structure.CountOfFieldsInFile);
                    }
                }
            }

            var filename = Path.GetFileName(filepath);
            if (!structures.Any())
            {
                prefix(filename).Wl(" structure wasn't found", ConsoleColor.Yellow);
                return default(T);
            }
            if (structures.Count > 1)
            {
                prefix(filename).Wl(string.Format(" There are {0} corresponding file structures.", structures.Count), ConsoleColor.Red);
                return default(T);
            }

            var first = structures.First();
            prefix(filename).Wl(string.Format("{0}", first.Name), ConsoleColor.Green);
            return first;
        }

        public bool Match(string filepath, IStructure structure)
        {
            if (!_fileMaskFilter.IsMatch(structure.FileMask, filepath))
            {
                return false;
            }
            using (var reader = FileReaderFactory.Create(filepath, structure))
            {
                reader.Skip(structure.SkipRows + structure.RowsInHeader);
                var fields = reader.ParseNextRow();
                if (fields.Length != structure.CountOfFieldsInFile)
                {
                    Lo.Wl("Structure " + structure.Name + " and file " + Path.GetFileName(filepath) + " are incompatible. Fields count = " +
                          fields.Length + " structure's fields count=" + structure.CountOfFieldsInFile);
                    return false;
                }
            }

            var filename = Path.GetFileName(filepath);
            prefix(filename).Wl(string.Format("{0}", structure.Name), ConsoleColor.Green);
            return true;
        }

        private Logger prefix(string file)
        {
            return Lo.W("Detect structure: ", ConsoleColor.Cyan).W(file).W(" - ");
        }
    }
}
