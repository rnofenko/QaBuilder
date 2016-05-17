using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Q2.Core.Structure;
using Q2.Core.Structure.Json;

namespace Qa.Core.Structure.Json
{
    public class JsonStructureLoader
    {
        public List<FileStructure> Load(string path)
        {
            var content = File.ReadAllText(path);
            var jsonStructures = JsonConvert.DeserializeObject<List<JsonFileStructure>>(content);
            return jsonStructures.Select(convert).ToList();
        }

        private FileStructure convert(JsonFileStructure json)
        {
            json.Format = json.Format ?? new JsonFormatStructure();

            var fields = json.Fields.Select(x => x.Convert()).ToList();
            var structure = new FileStructure
            {
                Fields = fields,
                Format = new FormatStructure
                {
                    Name = json.Name,
                    Fields = fields,
                    RowsInHeader = json.RowsInHeader ?? 0,
                    FileMask = json.Format.FileMask ?? "*.csv",
                    Delimiter = json.Format.Delimiter ?? ",",
                    TextQualifier = json.Format.TextQualifier ?? "\"",
                    CountOfFieldsInFile = fields.Count
                },
                Qa = new QaStructure
                {
                    Name = json.Name,
                    SourceFields = fields,
                    RowsInHeader = json.Qa.RowsInHeader ?? json.RowsInHeader ?? 0,
                    FileMask = json.Qa.FileMask ?? "*.txt",
                    Delimiter = json.Qa.Delimiter ?? "|",
                    Fields = json.Qa.Fields.Select(x => x.Convert(fields)).ToList(),
                    CountOfFieldsInFile = fields.Count
                }
            };
            structure.Format.DestinationDelimiter = structure.Qa.Delimiter;
            return structure;
        }
    }
}