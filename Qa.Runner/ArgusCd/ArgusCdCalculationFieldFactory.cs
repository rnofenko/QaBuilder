using System.Collections.Generic;
using Q2.Core.Structure;
using Qa.Core.Parsers.CalcFields;
using Qa.Core.Structure;

namespace Qa.Runner.ArgusCd
{
    public class ArgusCdCalculationFieldFactory : ICalculationFieldFactory
    {
        private readonly DefaultCalculationFieldFactory _default;

        public ArgusCdCalculationFieldFactory()
        {
            _default = new DefaultCalculationFieldFactory();
        }

        public ICalculationField Get(QaField field, List<Field> allFields)
        {
            if (field.Code == "totalMonthlyInterest")
            {
                return new TotalMonthkyInterestField(field);
            }

            return _default.Get(field, allFields);
        }
    }
}
