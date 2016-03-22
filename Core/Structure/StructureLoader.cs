using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Qa.Core.Structure
{
    public class StructureLoader
    {
        public ProjectStructure Load(string folder)
        {
            var path = Path.Combine(folder, "structure.json");
            if (!File.Exists(path))
            {
                return new ProjectStructure();
            }

            var content = File.ReadAllText(path);
            var project = JsonConvert.DeserializeObject<ProjectStructure>(content);
            project.Structures.ForEach(prepareForUse);
            project.FormatSchemes?.ForEach(x => prepareFormatScheme(x, project.Structures));
            return project;
        }

        private void prepareFormatScheme(FormatStructure formatStructure, List<FileStructure> structures)
        {
            var structure = structures.FirstOrDefault(x => x.Name == formatStructure.Name);
            if (structure == null)
            {
                return;
            }
            formatStructure.Destination = structure;
            formatStructure.Delimeter = formatStructure.Delimeter ?? (structure.Delimeter ?? ",");
            if (formatStructure.Fields.IsEmpty())
            {
                formatStructure.Fields = structure.Fields.Select(x=>new FormatFieldDescription(x)).ToList();
            }
        }

        private void prepareForUse(FileStructure structure)
        {
            structure.Fields.ForEach(checkField);
        }

        private void checkField(FieldDescription field)
        {
            if (field.Type == DType.None)
            {
                if (field.DateFormat.IsNotEmpty())
                {
                    field.Type = DType.Date;
                }
                else if (field.NumberFormat == NumberFormat.None)
                {
                    field.Type = DType.String;
                }
                else
                {
                    field.Type = DType.Number;
                }
            }

            if (field.NumberFormat == NumberFormat.None && field.Type == DType.Number)
            {
                field.NumberFormat = NumberFormat.Double;
            }
        }
    }
}
