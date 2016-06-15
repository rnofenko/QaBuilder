using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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

            var fields = json.FileFields.Select(x => x.Convert()).ToList();
            var structure = new FileStructure
            {
                Fields = fields
            };
            structure.Qa = getQa(json, structure);
            structure.Format = getFormat(json, structure);
            
            return structure;
        }

        private FormatStructure getFormat(JsonFileStructure json, FileStructure structure)
        {
            if (json.Qa != null)
            {
                return new FormatStructure();
            }

            var delimiter = json.Format.Delimiter ?? ",";
            var format = new FormatStructure
            {
                Name = json.Name,
                SourceFields = structure.Fields,
                RowsInHeader = json.Format.RowsInHeader ?? json.RowsInHeader ?? 0,
                FileMask = json.Format.FileMask ?? "*.csv",
                Delimiter = delimiter,
                TextQualifier = json.Format.TextQualifier ?? "\"",
                CountOfFieldsInFile = structure.Fields.Count,
                DestinationDelimiter = json.Format.DestinationDelimiter ?? structure.Qa.Delimiter ?? delimiter
            };
            return format;
        }

        private QaStructure getQa(JsonFileStructure json, FileStructure structure)
        {
            if (json.Qa == null)
            {
                return new QaStructure();
            }

            return new QaStructure
            {
                Name = json.Name,
                SourceFields = structure.Fields,
                RowsInHeader = json.Qa.RowsInHeader ?? json.RowsInHeader ?? 0,
                FileMask = json.Qa.FileMask ?? "*.txt",
                Delimiter = json.Qa.Delimiter ?? "|",
                Fields = json.Qa.Fields.Select(x => x.Convert(structure.Fields)).ToList(),
                CountOfFieldsInFile = structure.Fields.Count
            };
        }
    }
}