using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcCountField : CalcBaseField, ICalculationField
    {
        private int _count;

        public CalcCountField(QaField field)
            : base(field)
        {
        }

        public void Calc(string[] parts)
        {
            _count++;
        }

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