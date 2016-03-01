using System.Collections.Generic;

namespace Qa.Compare
{
    public class CompareResult
    {
        public CompareNumber RowsCount { get; set; }

        public List<CompareNumberField> Fields { get; set; }

        public string FileName { get; set; }

        public CompareResult()
        {
            Fields = new List<CompareNumberField>();
        }
    }
}