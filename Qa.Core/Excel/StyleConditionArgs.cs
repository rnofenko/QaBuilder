using Qa.Core.Excel;

namespace Q2.Core.Excel
{
    public class StyleConditionArgs
    {
        public Pos Pos { get; set; }

        public ExcelCursor Cursor { get; set; }

        public TypedValue Value { get; set; }
    }
}