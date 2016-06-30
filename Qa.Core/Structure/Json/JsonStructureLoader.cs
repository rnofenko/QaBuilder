using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Qa.Core.Compare;
using Qa.Core.Parsers;

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
            if (json.Format == null)
            {
                return new FormatStructure { Delimiter = CsvParser.DEFAULT_DELIMITER, DestinationDelimiter = CsvParser.DEFAULT_DELIMITER};
            }

            var delimiter = json.Format.Delimiter.IfEmpty(json.Delimiter).IfEmpty(",");
            if (delimiter.IsEmpty())
            {
                throw new WarningException(string.Format("Structure " + json.Name + " doesn't have delimiter for formatting."));
            }
            var format = new FormatStructure
            {
                Name = json.Name,
                SourceFields = structure.Fields,
                RowsInHeader = json.Format.RowsInHeader ?? json.RowsInHeader ?? 0,
                SkipRows = json.Format.SkipRows ?? json.SkipRows ?? 0,
                FileMask = json.Format.FileMask ?? "*.csv",
                Delimiter = delimiter,
                TextQualifier = json.Format.TextQualifier ?? "\"",
                DestinationDelimiter = json.Format.DestinationDelimiter ?? structure.Qa.Delimiter ?? delimiter
            };
            return format;
        }

        private QaStructure getQa(JsonFileStructure json, FileStructure structure)
        {
            if (json.Qa == null)
            {
                return new QaStructure { Delimiter = CsvParser.DEFAULT_DELIMITER};
            }
            
            return new QaStructure
            {
                Name = json.Name,
                SourceFields = structure.Fields,
                RowsInHeader = json.Qa.RowsInHeader ?? json.RowsInHeader ?? 0,
                SkipRows = json.Qa.SkipRows ?? json.SkipRows ?? 0,
                FileMask = json.Qa.FileMask ?? "*.csv",
                Delimiter = json.Qa.Delimiter.IfEmpty(json.Delimiter).IfEmpty(","),
                Fields = json.Qa.Fields.Select(x => x.Convert(structure.Fields)).ToList(),
                CompareFilesMethod = json.Qa.CompareFilesMethod == CompareFilesMethod.None ? CompareFilesMethod.MonthByMonth : json.Qa.CompareFilesMethod
            };
        }
    }
}