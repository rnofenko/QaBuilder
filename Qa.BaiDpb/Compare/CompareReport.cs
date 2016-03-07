using System.Collections.Generic;
using Qa.Core.Compares;

namespace Qa.BaiDpb.Compare
{
    public class CompareReport
    {
        public CompareNumber RowsCount { get; set; }

        public List<CompareNumberField> Fields { get; set; }

        public string FileName { get; set; }

        public CompareReport()
        {
            Fields = new List<CompareNumberField>();
        }
    }
}