using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Qa.Core.Structure
{
    public class StructureDetector
    {
        public StructureDetectResut Detect(string filepath, List<FileStructure> structures)
        {
            var result = new StructureDetectResut { FilePath = filepath };
            using (var stream = new StreamReader(filepath))
            {
                result.Line = stream.ReadLine();
            }
            if (result.Line.IsEmpty())
            {
                result.Error = "File is empty";
                return result;
            }
            tryAllDelimeters(result, structures);
            
            return result;
        }

        private void tryAllDelimeters(StructureDetectResut result, IEnumerable<FileStructure> sourceStructures)
        {
            var structures = new List<FileStructure>();

            foreach (var structure in sourceStructures.Where(x=>x.SourceDelimeter.IsNotEmpty()))
            {
                var fields = result.Line.Split(new[] { structure.SourceDelimeter }, StringSplitOptions.None);
                if (fields.Length == structure.Fields.Count)
                {
                    structures.Add(structure);
                }
            }
            
            if (!structures.Any())
            {
                result.Error = "Corresponding file structure wasn't found.";
            }
            else if (structures.Count > 1)
            {
                result.Error = $"There are {structures.Count} corresponding file structures.";
            }
            else
            {
                result.Structure = structures.First();
            }
        }
    }
}
