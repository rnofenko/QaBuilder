using Qa.Core.Calculations;

namespace Qa.Core.Compare
{
    public class CompareNumber
    {
        public CompareNumber(double? current, double? previous)
        {
            Current = current;
            Previous = previous;
            if (previous != null && current != null)
            {
                PercentChange = Calculator.ChangeInPercent(current.Value, Previous);
                AbsChange = Calculator.AbsoluteChange(current.Value, Previous);
            }
        }

        public double? Previous { get; set; }

        public double? Current { get; set; }
        
        public double? PercentChange { get; set; }

        public double? AbsChange { get; set; }
    }
}