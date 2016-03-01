using System.Collections.Generic;
using Qa.Structure;

namespace Qa.Compare
{
    public class ComparePacket
    {
        public FileStructure Strucure { get; set; }

        public List<CompareResult> Files { get; set; }

        public ComparePacket()
        {
            Files = new List<CompareResult>();
        }
    }
}