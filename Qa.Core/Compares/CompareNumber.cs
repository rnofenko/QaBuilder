using Qa.Core.Calculations;

namespace Qa.Core.Compares
{
    public class CompareNumber
    {
        public CompareNumber(double current, double? previous)
        {
            Current = current;
            if (previous != null)
            {
                Previous = previous.Value;
                Change = Calculator.ChangeInPercent(current, Previous);
            }
        }

        public double Previous { get; set; }

        public double Current { get; set; }

        public double Change { get; set; }
    }
}