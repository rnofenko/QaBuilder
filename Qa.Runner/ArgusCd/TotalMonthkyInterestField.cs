using System.Collections.Generic;
using Q2.Core.Collectors;
using Q2.Core.Collectors.CalcFields;
using Q2.Core.Structure;

namespace Qa.Runner.ArgusCd
{
    public class TotalMonthkyInterestField : ICalculationField
    {
        private int _balanceIndex = 9;
        private int _apyIndex = 10;
        private double _interest;

        public TotalMonthkyInterestField(QaField field)
        {
            Field = field;
        }

        public void Calc(string[] parts)
        {
            var balance = NumberParser.Parse(parts[_balanceIndex]);
            var apy = NumberParser.Parse(parts[_apyIndex]);
            _interest += apy/12*balance;
        }

        public QaField Field { get; private set; }

        public double GetSingleResult()
        {
            return _interest;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return null;
        }
    }
}