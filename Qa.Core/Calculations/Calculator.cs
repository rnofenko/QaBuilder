using System;

namespace Qa.Core.Calculations
{
    public static class Calculator
    {
        public static double? ChangeInPercent(double current, double previous)
        {
            if (!(Math.Abs(previous) > 0.0001d) && !(Math.Abs(current) > 0.0001d))
            {
                return 0;
            }

            if (Math.Abs(previous) < 0.0001d)
            {
                return null;
            }
            if (Math.Abs(current) < 0.0001d)
            {
                return -1;
            }
            return current * 1.0/ previous - 1;
        }

        public static double AbsoluteChange(double current, double previous)
        {
            return Math.Abs(current - previous);
        }
    }
}