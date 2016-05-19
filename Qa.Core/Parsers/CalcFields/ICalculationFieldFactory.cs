using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public interface ICalculationFieldFactory
    {
        ICalculationField Get(QaField field, List<Field> allFields);
    }
}