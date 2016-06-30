using System.Collections.Generic;
using Qa.Core.Compare;

namespace Qa.Core.Structure.Json
{
    public class JsonQaStructure
    {
        public CompareFilesMethod CompareFilesMethod { get; set; }

        public string Delimiter { get; set; }

        public List<JsonQaField> Fields { get; set; }

        public string FileMask { get; set; }

        public int? RowsInHeader { get; set; }

        public int? SkipRows { get; set; }
    }
}