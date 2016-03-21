using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class Settings
    {
        public string WorkingFolder { get; set; }

        public List<FileStructure> FileStructures => Project.Structures;

        public ProjectStructure Project { get; set; }
    }
}