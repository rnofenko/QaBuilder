using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class FormatStructure
    {
        public string Name { get; set; }

        public int? RowsInHeader { get; set; }

        public string Delimiter { get; set; }

        public List<FormatFieldDescription> Fields { get; set; }

        public List<FormatFieldDescription> CalculatedFields { get; set; }

        public FileStructure Destination { get; set; }
    }
}