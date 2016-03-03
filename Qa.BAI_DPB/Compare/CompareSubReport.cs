using System.Collections.Generic;

namespace Qa.BAI_DPB.Compare
{
    public class CompareSubReport
    {
        public string State { get; set; }

        public CompareNumber RowsCount { get; set; }

        public List<CompareNumberField> Fields { get; set; }

        public string FileName { get; set; }

        public CompareSubReport()
        {
            Fields = new List<CompareNumberField>();
        }
    }
}