using System.Collections.Generic;
using Qa.Core.Structure.Json;

namespace Q2.Core.Structure.Json
{
    public class JsonFileStructure
    {
        public string Name { get; set; }
        
        public int? RowsInHeader { get; set; }

        public List<JsonField> Fields { get; set; }
        
        public JsonQaStructure Qa { get; set; }

        public JsonFormatStructure Format { get; set; }
    }
}