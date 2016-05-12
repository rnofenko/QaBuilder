using System.Collections.Generic;
using Q2.Core.Structure;

namespace Qa.Core.Structure
{
    public class FileStructure
    {
        public List<Field> Fields { get; set; }

        public QaStructure Qa { get; set; }

        public FormatStructure Format { get; set; }
    }
}