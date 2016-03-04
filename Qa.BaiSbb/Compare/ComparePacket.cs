using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.BaiSbb.Compare
{
    public class ComparePacket
    {
        public FileStructure Structure { get; set; }

        public List<CompareReport> Reports { get; set; }

        public ComparePacket()
        {
            Reports = new List<CompareReport>();
        }
    }
}