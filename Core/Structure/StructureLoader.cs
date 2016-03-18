using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Qa.Core.Structure
{
    public class StructureLoader
    {
        public List<FileStructure> Load(string folder)
        {
            var path = Path.Combine(folder, "structure.json");
            if (!File.Exists(path))
            {
                return new List<FileStructure>();
            }

            var content = File.ReadAllText(path);
            var structures = JsonConvert.DeserializeObject<List<FileStructure>>(content);
            structures.ForEach(prepareForUse);
            return structures;
        }

        private void prepareForUse(FileStructure structure)
        {
            structure.Fields.ForEach(checkField);
            if (structure.DestinationDelimeter.IsEmpty())
            {
                structure.DestinationDelimeter = structure.SourceDelimeter;
            }
        }

        private void checkField(FieldDescription field)
        {
            if (field.Type == DType.None)
            {
                if (field.Format == FormatType.None)
                {
                    field.Type = DType.String;
                }
                else
                {
                    field.Type = DType.Number;
                }
            }

            if (field.Format == FormatType.None && field.Type == DType.Number)
            {
                field.Format = FormatType.Double;
            }
        }
    }
}
