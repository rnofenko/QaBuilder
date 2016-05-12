using System.Collections.Generic;
using Q2.Core.Collectors;
using Q2.Core.Collectors.CalcFields;
using Q2.Core.Structure;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcWeightedAverageField : CalcBaseField, ICalculationField
    {
        private const double FAKE_SUM = 1000000;

        private double _sum;
        private double _fakeWeightedAverage;
        private readonly int _index;
        private readonly int _weightIndex;

        public CalcWeightedAverageField(QaField field)
            :base(field)
        {
            _index = field.FieldIndex;
            _weightIndex = field.WeightFieldIndex;
        }

        public void Calc(string[] parts)
        {
            var value = NumberParser.Parse(parts[_index]);
            var weightAmount = NumberParser.Parse(parts[_weightIndex]);
            _fakeWeightedAverage += weightAmount / FAKE_SUM * value;
            _sum += weightAmount;
        }

        public double GetSingleResult()
        {
            var fakeRate = FAKE_SUM/_sum;
            return _fakeWeightedAverage * fakeRate;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return null;
        }
    }
}