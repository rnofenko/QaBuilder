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

        public bool Match(string filepath, IStructure structure)
        {
            if (!_fileMaskFilter.IsMatch(structure.FileMask, filepath))
            {
                return false;
            }
            using (var reader = FileReaderFactory.Create(filepath, structure.GetLineParser()))
            {
                reader.Skip(structure.SkipRows + structure.RowsInHeader);
                var fields = reader.ParseNextRow();
                if (fields.Length != structure.CountOfSourceFields)
                {
                    const string template = "Structure {0} and file {1} are incompatible. Fields count =  {2} structure's fields count = {3}";
                    Lo.Wl(string.Format(template, structure.Name, Path.GetFileName(filepath), fields.Length, structure.CountOfSourceFields));
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
