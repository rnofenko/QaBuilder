using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class FileStructure: IStructure
    {
        public string Name { get; set; }

        public int RowsInHeader { get; set; }

        public string Delimiter { get; set; }

        public string TextQualifier { get; set; }

        public int FieldsCount => Fields.Count;

        public List<FieldDescription> Fields { get; set; }

        public List<FieldDescription> NewFields { get; set; }

        public List<TransformStructure> Transformations { get; set; }

        public FileStructure()
        {
            Transformations = new List<TransformStructure>();
        }
    }
}