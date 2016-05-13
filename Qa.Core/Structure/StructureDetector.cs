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
                    if (reader.GetFieldsCount() == structure.CountOfFieldsInFile)
                    {
                        structures.Add(structure);
                    }
                }
            }

            if (!structures.Any())
            {
                return default(T);
            }
            if (structures.Count > 1)
            {
                Lo.Wl().Wl(string.Format("ERROR  in {0}: There are {1} corresponding file structures.", filepath, structures.Count));
                return default(T);
            }
            return structures.First();
        }
    }
}
