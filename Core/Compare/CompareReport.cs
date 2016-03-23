using System.Collections.Generic;

namespace Qa.Core.Compare
{
    public class CompareReport
    {
        public CompareNumber RowsCount { get; set; }

        public List<CompareField> Numbers { get; set; }

        public string FileName { get; set; }

        public int Index { get; set; }

        public CompareReport()
        {
            Numbers = new List<CompareField>();
        }
    }
}