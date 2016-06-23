using System.Collections.Generic;
using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public interface INumberFieldPrinter
    {
        void Print(IEnumerable<NumberField> fields, ExcelCursor cursor, ComparePacket packet);
    }
}