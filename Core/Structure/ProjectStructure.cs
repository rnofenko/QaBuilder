using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class ProjectStructure
    {
        public List<FileStructure> Structures { get; set; }

        public List<FormatStructure> FormatSchemes { get; set; }
    }
}