using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Qa.Core.Structure
{
    public class StructureLoader
    {
        public List<FileStructure> Load(string folder)
        {
            var content = File.ReadAllText(folder + "/structure.json");
            var structures = JsonConvert.DeserializeObject<List<FileStructure>>(content);
            structures.ForEach(prepareForUse);
            return structures;
        }

        private void prepareForUse(FileStructure structure)
        {
            structure.Fields.ForEach(checkField);
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
