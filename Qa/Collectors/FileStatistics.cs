using System.Collections.Generic;
using Qa.Structure;

namespace Qa.Collectors
{
    public class FileStatistics
    {
        public string Path { get; set; }

        public string Error { get; set; }

        public int RowsCount { get; set; }

        public FileStructure Structure { get; set; }

        public int FieldsCount { get; set; }

        public List<StatisticsField> Fields { get; set; }

        public FileStatistics()
        {
            Fields = new List<StatisticsField>();
        }
    }
}