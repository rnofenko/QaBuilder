using Q2.Core.Collectors;
using Q2.Core.Collectors.CalcFields;
using Qa.Core.Parsers.CalcFields;

namespace Q2.Core.System
{
    public class ServiceLocator
    {
        private static ICalculationFieldFactory _calculationFieldFactory;
        public static ICalculationFieldFactory CalculationFieldFactory
        {
            get { return _calculationFieldFactory ?? (_calculationFieldFactory = new DefaultCalculationFieldFactory()); }
            set { _calculationFieldFactory = value; }
        }
    }
}
