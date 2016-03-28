using Qa.Core.Calculations;
using Qa.Core.Excel;
using Qa.Core.Structure;

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
                Change = Calculator.ChangeInPercent(current, Previous);
                AbsoluteChange = Calculator.AbsoluteChange(current, Previous);
            }
        }

        public double Previous { get; set; }

        public double Current { get; set; }
        
        public double? Change { get; set; }

        public double AbsoluteChange { get; set; }

        public TypedValue CurrentAsInteger => new TypedValue(Current, NumberFormat.Integer);

        public TypedValue ChangeAsPercent => new TypedValue(Change, NumberFormat.Percent);
    }
}