using OfficeOpenXml;
using Q2.Core.Compare;

namespace Q2.Core.Excel
{
    public interface IExportPage
    {
        void Print(ComparePacket packet, ExcelWorksheet sheet);
    }
}