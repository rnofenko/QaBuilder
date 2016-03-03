using System.Collections.Generic;

namespace Qa.Compare
{
    public class CompareSubReport
    {
        public string Key { get; set; }

        public CompareNumber RowsCount { get; set; }

        public List<CompareNumberField> Fields { get; set; }

        public string FileName { get; set; }

        public CompareSubReport()
        {
            Fields = new List<CompareNumberField>();
        }
    }
}