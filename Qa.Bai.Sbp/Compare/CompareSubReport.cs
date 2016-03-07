using System.Collections.Generic;
using Qa.Core.Compare;

namespace Qa.Sbpm.Compare
{
    public class CompareSubReport
    {
        public string State { get; set; }

        public CompareNumber RowsCount { get; set; }

        public List<CompareField> Fields { get; set; }

        public string FileName { get; set; }

        public CompareSubReport()
        {
            Fields = new List<CompareField>();
        }
    }
}