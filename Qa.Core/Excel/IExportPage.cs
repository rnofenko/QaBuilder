using OfficeOpenXml;
using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public interface IExportPage
    {
        void Print(string structureName, ComparePacket packet, ExcelWorksheet sheet);
    }
}