using System.Collections.Generic;
using Qa.Structure;

namespace Qa.System
{
    public class StructureDetectSettings
    {
        public string Delimeter { get; set; }

        public List<FileStructure> FileStructures { get; set; }
    }
}