using System.Collections.Generic;
using Qa.Structure;

namespace Qa.System
{
    public class Settings
    {
        public string WorkingFolder { get; set; }

        public List<FileStructure> FileStructures { get; set; }
    }
}