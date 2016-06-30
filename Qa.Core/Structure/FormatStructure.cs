using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class FormatStructure : IStructure
    {
        public FormatStructure()
        {
            SourceFields = new List<Field>();
        }

        public string Name { get; set; }

        public int RowsInHeader { get; set; }

        public string FileMask { get; set; }

        public int CountOfSourceFields { get { return SourceFields.Count; } }

        public int SkipRows { get; set; }

        public string Delimiter { get; set; }

        public List<Field> SourceFields { get; set; }

        public string TextQualifier { get; set; }

        public string DestinationDelimiter { get; set; }
    }
}