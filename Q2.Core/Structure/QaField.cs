using System.Collections.Generic;
using Qa.Core.Structure;

namespace Q2.Core.Structure
{
    public class QaField
    {
        public string Title { get; set; }

        public NumberFormat NumberFormat { get; set; }

        public SortType Sort { get; set; }

        public Dictionary<string, string> Translate { get; set; }

        public FieldStyle Style { get; set; }

        public List<BinRange> Bins { get; set; }

        public bool Group { get; set; }

        public int FieldIndex { get; set; }

        public CalculationType Calculation { get; set; }

        public int WeightFieldIndex { get; set; }

        public string Code { get; set; }

        public string FilterExpression { get; set; }

        public QaField(QaField qa)
        {
            Calculation = qa.Calculation;
            Code = qa.Code;
            FieldIndex = qa.FieldIndex;
            NumberFormat = qa.NumberFormat;
            Sort = qa.Sort;
            Style = qa.Style;
            Translate = qa.Translate;
            Title = qa.Title;
            WeightFieldIndex = qa.WeightFieldIndex;
        }

        public QaField()
        {
        }
    }
}