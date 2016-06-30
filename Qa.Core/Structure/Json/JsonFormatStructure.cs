using System.Collections.Generic;

namespace Qa.Core.Structure.Json
{
    public class JsonFormatStructure
    {
        public string Delimiter { get; set; }

        public string FileMask { get; set; }

        public string TextQualifier { get; set; }

        public List<JsonFormatField> Fields { get; set; }

        public int? RowsInHeader { get; set; }

        public string DestinationDelimiter { get; set; }

        public int? SkipRows { get; set; }
    }
}