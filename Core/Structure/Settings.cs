using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class Settings
    {
        public string WorkingFolder { get; set; }

        public List<FileStructure> FileStructures {get { return Project.Structures; } }

        public ProjectStructure Project { get; set; }

        public string QaFileName { get; set; }

        public string FileMask { get; set; }

        public Settings(string fileMask = "*.txt")
        {
            FileMask = fileMask;
        }
    }
}