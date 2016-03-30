using OfficeOpenXml;
using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public interface IExportPage
    {
        void Print(ComparePacket packet, ExcelWorksheet sheet);
    }
}