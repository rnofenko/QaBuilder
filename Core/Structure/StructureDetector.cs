using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.System;

namespace Qa.Core.Structure
{
    public class StructureDetector
    {
        public T Detect<T>(string filepath, IEnumerable<T> sourceStructures) where T : IStructure
        {
            var structures = new List<T>();

            foreach (var structure in sourceStructures)
            {
                string line;
                using (var stream = new StreamReader(filepath))
                {
                    for (var i = 0; i < structure.SkipRows + structure.RowsInHeader; i++)
                    {
                        stream.ReadLine();
                    }
                    line = stream.ReadLine();
                }
                if (line.IsEmpty())
                {
                    continue;
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
                Lo.Wl().Wl(string.Format("ERROR  in {0}: There are {1} corresponding file structures.", filepath, structures.Count));
                return default(T);
            }
            return structures.First();
        }
    }
}
