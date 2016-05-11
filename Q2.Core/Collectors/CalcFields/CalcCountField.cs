using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Collectors.CalcFields
{
    public class CalcCountField : ICalculationField
    {
        private int _count;

        public CalcCountField(QaField field)
        {
            Field = field;
        }

        public void Calc(string[] parts)
        {
            _count++;
        }

        public QaField Field { get; }

        public double GetSingleResult()
        {
            return _count;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return null;
        }
    }
}