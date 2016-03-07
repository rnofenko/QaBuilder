using Qa.Core.Calculations;

namespace Qa.BaiSbb.Compare
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