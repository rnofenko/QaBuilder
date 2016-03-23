using System.Collections.Generic;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public interface IExporter
    {
        void Export(List<ComparePacket> packets, Settings settings);
    }
}
