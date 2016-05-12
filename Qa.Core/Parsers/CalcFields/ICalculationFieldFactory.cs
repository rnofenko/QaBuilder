using System.Collections.Generic;
using Q2.Core.Structure;

namespace Q2.Core.Collectors.CalcFields
{
    public interface ICalculationFieldFactory
    {
        ICalculationField Get(QaField field, List<Field> allFields);
    }
}