using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class Settings
    {
        public string WorkingFolder { get; set; }

        public List<FileStructure> FileStructures { get; set; }
    }
}