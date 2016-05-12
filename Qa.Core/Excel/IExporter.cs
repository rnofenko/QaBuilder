using System.Collections.Generic;
using Q2.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public interface IExporter
    {
        void Export(List<ComparePacket> packets, Settings settings);
    }
}