using System.Collections.Generic;
using Qa.Core.Structure;

namespace Q2.Core.Structure
{
    public class FileStructure
    {
        public List<Field> Fields { get; set; }

        public QaStructure Qa { get; set; }

        public FormatStructure Format { get; set; }
    }
}