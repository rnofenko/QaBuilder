using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.BAI_DPB.Compare
{
    public class ComparePacket
    {
        public FileStructure Strucure { get; set; }

        public List<CompareReport> Reports { get; set; }

        public ComparePacket()
        {
            Reports = new List<CompareReport>();
        }
    }
}