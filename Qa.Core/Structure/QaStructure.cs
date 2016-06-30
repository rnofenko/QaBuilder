using System.Collections.Generic;
using Qa.Core.Compare;

namespace Qa.Core.Structure
{
    public class QaStructure: IStructure
    {
        public QaStructure()
        {
            SourceFields = new List<Field>();
        }

        public string Name { get; set; }

        public string Delimiter { get; set; }

        public string TextQualifier { get; set; }

        public int SkipRows { get; set; }

        public int RowsInHeader { get; set; }

        public string FileMask { get; set; }

        public int CountOfSourceFields
        {
            get { return SourceFields.Count; }
        }

        public List<QaField> Fields { get; set; }

        public List<Field> SourceFields { get; set; }

        public CompareFilesMethod CompareFilesMethod { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}