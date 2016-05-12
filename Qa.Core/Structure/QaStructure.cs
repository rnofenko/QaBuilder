using System.Collections.Generic;
using Q2.Core.Structure;

namespace Qa.Core.Structure
{
    public class QaStructure: IStructure
    {
        public string Name { get; set; }

        public string Delimiter { get; set; }

        public string TextQualifier { get; set; }

        public int SkipRows { get; set; }

        public int RowsInHeader { get; set; }

        public string FileMask { get; set; }

        public int CountOfFieldsInFile { get; set; }

        public List<QaField> Fields { get; set; }

        public List<Field> SourceFields { get; set; }
    }
}