using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Collectors.CalcFields
{
    public interface ICalculationField
    {
        void Calc(string[] parts);

        QaField Field { get; }

        double GetSingleResult();

        Dictionary<string, double> GetGroupedResult();
    }
}