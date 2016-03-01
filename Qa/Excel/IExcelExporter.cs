using System.Collections.Generic;
using Qa.Compare;

namespace Qa.Excel
{
    public interface IExcelExporter
    {
        void Export(List<ComparePacket> packets, CompareSettings settings);
    }
}