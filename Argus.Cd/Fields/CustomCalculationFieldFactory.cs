using System.Collections.Generic;
using Q2.Core.Collectors;
using Q2.Core.Collectors.CalcFields;
using Q2.Core.Structure;

namespace Q2.Argus.Cd.Fields
{
    public class CustomCalculationFieldFactory : ICalculationFieldFactory
    {
        private readonly DefaultCalculationFieldFactory _default;

        public CustomCalculationFieldFactory()
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
