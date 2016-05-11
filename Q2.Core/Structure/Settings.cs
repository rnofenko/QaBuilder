using System.Collections.Generic;

namespace Q2.Core.Structure
{
    public class Settings
    {
        public string WorkingFolder { get; set; }

        public List<FileStructure> FileStructures { get; set; }

        public string QaFileName { get; set; }
    }
}