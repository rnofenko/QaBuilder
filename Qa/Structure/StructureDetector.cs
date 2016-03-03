using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Extensions;

namespace Qa.Structure
{
    public class StructureDetector
    {
        public StructureDetectResut Detect(string filepath, StructureDetectSettings settings)
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
            
            if (settings.Delimeter.IsNotEmpty())
            {
                useOneDelimeterForAll(result, settings);
            }
            else
            {
                tryAllDelimeters(result, settings);
            }

            return result;
        }

        private void tryAllDelimeters(StructureDetectResut result, StructureDetectSettings settings)
        {
            var structures = new List<FileStructure>();

            foreach (var structure in settings.FileStructures.Where(x=>x.Delimeter.IsNotEmpty()))
            {
                var fields = result.Line.Split(new[] { structure.Delimeter }, StringSplitOptions.None);
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

        private void useOneDelimeterForAll(StructureDetectResut result, StructureDetectSettings settings)
        {
            var fields = result.Line.Split(new[] { settings.Delimeter }, StringSplitOptions.None);
            result.FieldsCount = fields.Length;

            var structures = settings.FileStructures.Where(x => x.Fields.Count == fields.Length).ToList();
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
