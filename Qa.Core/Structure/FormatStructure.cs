using System.Collections.Generic;
using Q2.Core.Structure;

namespace Qa.Core.Structure
{
    public class FormatStructure : IStructure
    {
        public string Name { get; set; }

        public int RowsInHeader { get; set; }

        public string FileMask { get; set; }

        public int CountOfFieldsInFile { get; set; }

        public int SkipRows { get; set; }

        public string Delimiter { get; set; }

        public List<Field> Fields { get; set; }

        public string TextQualifier { get; set; }

        public string DestinationDelimiter { get; set; }
    }
}