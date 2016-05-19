using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public interface ICalculationField
    {
        void Calc(string[] parts);

        QaField Field { get; }

        double GetSingleResult();

        Dictionary<string, double> GetGroupedResult();
    }
}