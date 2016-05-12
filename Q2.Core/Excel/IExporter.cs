using System.Collections.Generic;
using Q2.Core.Compare;
using Q2.Core.Structure;

namespace Q2.Core.Excel
{
    public interface IExporter
    {
        void Export(List<ComparePacket> packets, Settings settings);
    }
}