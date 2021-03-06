﻿using System.Collections.Generic;

namespace Qa.Core.Structure.Json
{
    public class JsonFileStructure
    {
        public string Name { get; set; }

        public string Delimiter { get; set; }
        
        public int? RowsInHeader { get; set; }

        public int? SkipRows { get; set; }

        public List<JsonField> FileFields { get; set; }
        
        public JsonQaStructure Qa { get; set; }

        public JsonFormatStructure Format { get; set; }

        public string FileMask { get; set; }
    }
}