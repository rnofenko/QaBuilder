using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class FileStructure
    {
        public string Name { get; set; }

        public int RowsInHeader { get; set; }

        public List<FieldDescription> Fields { get; set; }

        public List<TransformStructure> Transformations { get; set; }

        public string Delimeter { get; set; }

        public FileStructure()
        {
            Transformations = new List<TransformStructure>();
        }
    }
}