using System;

namespace Qa.Sbpm.Compare
{
    public class CompareNumber
    {
        public CompareNumber(int current, int? previous)
        {
            Current = current;
            if (previous != null)
            {
                Previous = previous.Value;
                Increase = Math.Round(current * 1.0/Previous*100 - 100);
            }
        }

        public int Previous { get; set; }

        public int Current { get; set; }

        public double Increase { get; set; }
    }
}