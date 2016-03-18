using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class FileStructure
    {
        public string Name { get; set; }

        public int RowsInHeader { get; set; }

        public string SourceDelimeter { get; set; }

        public string DestinationDelimeter { get; set; }

        public List<FieldDescription> Fields { get; set; }

        public List<FieldDescription> NewFields { get; set; }

        public List<TransformStructure> Transformations { get; set; }

        public FileStructure()
        {
            Transformations = new List<TransformStructure>();
        }
    }
}