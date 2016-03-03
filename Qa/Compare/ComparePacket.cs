using System.Collections.Generic;
using Qa.Structure;

namespace Qa.Compare
{
    public class ComparePacket
    {
        public FileStructure Strucure { get; set; }

        public List<CompareReport> Reports { get; set; }

        public List<string> AllKeys { get; set; }

        public ComparePacket()
        {
            Reports = new List<CompareReport>();
        }
    }
}