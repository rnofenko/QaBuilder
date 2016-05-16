using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class CalcGroupCountField : CalcBaseGroupField, ICalculationField
    {
        private readonly Dictionary<string, double> _groupedNumbers;
        
        public CalcGroupCountField(QaField field)
            : base(field)
        {
            _groupedNumbers = new Dictionary<string, double>();
        }

        public void Calc(string[] parts)
        {
            var key = GetKey(parts);
            try
            {
                _groupedNumbers[key]++;
            }
            catch
            {
                _groupedNumbers.Add(key, 1);
            }
        }

        public double GetSingleResult()
        {
            return 0;
        }

        public Dictionary<string, double> GetGroupedResult()
        {
            return _groupedNumbers.ToDictionary(x => x.Key.Replace(SEPARATOR, " / "), x => x.Value);
        }
    }
}