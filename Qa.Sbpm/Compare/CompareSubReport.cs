using System.Collections.Generic;
using Qa.Core.Compares;

namespace Qa.Sbpm.Compare
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