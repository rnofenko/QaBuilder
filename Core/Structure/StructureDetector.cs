using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.System;

namespace Qa.Core.Structure
{
    public class StructureDetector
    {
        public FormatStructure Detect(string filepath, List<FormatStructure> sourceStructures, bool showError = false)
        {
            using (var stream = new StreamReader(filepath))
            {
                var line = stream.ReadLine();
                if (line == null || line.IsEmpty())
                {
                    if (showError)
                    {
                        Lo.Wl().Wl($"ERROR  in {filepath}: File is empty");
                    }
                    return null;
                }

                var structures = new List<FormatStructure>();

                foreach (var structure in sourceStructures.Where(x => x.Delimiter.IsNotEmpty()))
                {
                    var fields = line.Split(new[] { structure.Delimiter }, StringSplitOptions.None);
                    if (fields.Length == structure.Fields.Count)
                    {
                        structures.Add(structure);
                    }
                }

                if (!structures.Any())
                {
                    if (showError)
                    {
                        Lo.Wl().Wl($"ERROR  in {filepath}: Corresponding file structure wasn't found.");
                    }
                    return null;
                }
                else if (structures.Count > 1)
                {
                    if (showError)
                    {
                        Lo.Wl().Wl($"ERROR  in {filepath}: There are {structures.Count} corresponding file structures.");
                    }
                    return null;
                }
                return structures.First();
            }
        }

        public StructureDetectResut Detect(string filepath, List<FileStructure> structures, bool showError = false)
        {
            var result = new StructureDetectResut { FilePath = filepath };
            using (var stream = new StreamReader(filepath))
            {
                result.Line = stream.ReadLine();
            }
            if (result.Line.IsEmpty())
            {
                if (showError)
                {
                    Lo.Wl().Wl($"ERROR  in {filepath}: File is empty");
                }
                return null;
            }
            tryAllDelimeters(result, structures);

            return result;
        }

        private void tryAllDelimeters(StructureDetectResut result, IEnumerable<FileStructure> sourceStructures, bool showError = false)
        {
            var structures = new List<FileStructure>();

            foreach (var structure in sourceStructures.Where(x => x.Delimiter.IsNotEmpty()))
            {
                var fields = result.Line.Split(new[] { structure.Delimiter }, StringSplitOptions.None);
                if (fields.Length == structure.Fields.Count)
                {
                    structures.Add(structure);
                }
            }

            if (!structures.Any())
            {
                if (showError)
                {
                    Lo.Wl().Wl($"ERROR  in {result.FilePath}: Corresponding file structure wasn't found.");
                }
                return;
            }
            else if (structures.Count > 1)
            {
                if (showError)
                {
                    Lo.Wl().Wl($"ERROR  in {result.FilePath}: There are {structures.Count} corresponding file structures.");
                }
                return;
            }
            else
            {
                result.Structure = structures.First();
            }
        }
    }
}
