using OfficeOpenXml;
using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public interface IExportPage
    {
        void PrintReport(ComparePacket packet, ExcelWorksheet sheet);
    }
}