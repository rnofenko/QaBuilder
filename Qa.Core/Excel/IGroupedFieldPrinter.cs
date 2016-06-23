using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public interface IGroupedFieldPrinter
    {
        void Print(GroupedField field, ExcelCursor cursor, ComparePacket packet);
    }
}