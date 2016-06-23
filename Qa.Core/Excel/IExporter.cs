using System;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public interface IExporter : IDisposable
    {
        void AddData(string structureName, ComparePacket packet, Settings settings);

        void Export();
    }
}