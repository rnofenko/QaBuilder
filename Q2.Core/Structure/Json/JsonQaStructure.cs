using System.Collections.Generic;

namespace Q2.Core.Structure.Json
{
    public class JsonQaStructure
    {
        public string Delimiter { get; set; }

        public List<JsonQaField> Fields { get; set; }

        public string FileMask { get; set; }
    }
}