using Qa.Core.Structure;

namespace Qa.Core.Excel
{
    public class StyleConditionArgs
    {
        public Pos Pos { get; set; }

        public ExcelCursor Cursor { get; set; }

        public DType Type { get; set; }

        public double Amount { get; set; }
    }
}