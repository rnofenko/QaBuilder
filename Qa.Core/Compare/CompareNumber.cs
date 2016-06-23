using Qa.Core.Calculations;

namespace Qa.Core.Compare
{
    public class CompareNumber
    {
        public CompareNumber(double current, double? previous)
        {
            Current = current;
            if (previous != null)
            {
                Previous = previous.Value;
                PercentChange = Calculator.ChangeInPercent(current, Previous);
                AbsChange = Calculator.AbsoluteChange(current, Previous);
            }
        }

        public double Previous { get; set; }

        public double Current { get; set; }
        
        public double? PercentChange { get; set; }

        public double AbsChange { get; set; }
    }
}