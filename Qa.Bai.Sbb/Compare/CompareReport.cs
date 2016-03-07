using System.Collections.Generic;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Bai.Sbb.Compare
{
    public class CompareReport
    {
        public CompareNumber RowsCount { get; set; }

        public List<CompareField> Numbers { get; set; }

        public List<CompareField> UniqueFields { get; set; }

        public string FileName { get; set; }

        public List<RawReportField> DictionaryFields { get; set; }

        public CompareReport()
        {
            Numbers = new List<CompareField>();
            DictionaryFields = new List<RawReportField>();
        }
    }
}