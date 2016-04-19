using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class FormatStructure : IStructure
    {
        public string Name { get; set; }

        public int? RowsInHeader { get; set; }

        public int? SkipRows { get; set; }

        public string Delimiter { get; set; }

        public List<FormatFieldDescription> Fields { get; set; }

        public List<FormatFieldDescription> CalculatedFields { get; set; }

        public FileStructure Destination { get; set; }

        public string TextQualifier { get; set; }

        public int FieldsCount
        {
            get { return Fields.Count; }
        }
    }
}