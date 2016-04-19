using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.System;

namespace Qa.Core.Structure
{
    public class StructureDetector
    {
        public T Detect<T>(string filepath, List<T> structures) where T : IStructure
        {
            using (var stream = new StreamReader(filepath))
            {
                var line = stream.ReadLine();
                return line.IsEmpty() ? default(T) : findStructure(line, filepath, structures);
            }
        }

        private T findStructure<T>(string line, string filepath, IEnumerable<T> sourceStructures) where T : IStructure
        {
            var structures = new List<T>();

            foreach (var structure in sourceStructures)
            {
                if (structure.SkipRows > 0)
                {
                    using (var stream = new StreamReader(filepath))
                    {
                        for (var i = 0; i < structure.SkipRows; i++)
                        {

                            stream.ReadLine();
                        }
                        line = stream.ReadLine();
                    }
                }

                var fields = structure.GetLineParser().Parse(line);
                if (fields.Length == structure.FieldsCount)
                {
                    structures.Add(structure);
                }
            }

            if (!structures.Any())
            {
                return default(T);
            }
            if (structures.Count > 1)
            {
                Lo.Wl().Wl("ERROR  in {filepath}: There are {structures.Count} corresponding file structures.");
                return default(T);
            }
            return structures.First();
        }
    }
}
