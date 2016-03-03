using System.Collections.Generic;

namespace Qa.Structure
{
    public class FileStructure
    {
        public string Name { get; set; }

        public string SubReportBy { get; set; }

        public int RowsInHeader { get; set; }

        public List<FieldDescription> Fields { get; set; }

        public string Delimeter { get; set; }
    }
}
