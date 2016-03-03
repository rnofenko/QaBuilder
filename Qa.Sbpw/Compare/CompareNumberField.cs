using System;
using Qa.Collectors;
using Qa.Structure;

namespace Qa.Sbpm.Compare
{
    public class CompareNumberField
    {
        public CompareNumberField(RawReportField current, RawReportField previous)
        {
            Name = current.Name;
            Type = current.Type;
            CurrentSum = current.Sum;
            if (previous != null)
            {
                PreviousSum = previous.Sum;
                if (Math.Abs(PreviousSum) > 0.0001d || Math.Abs(CurrentSum) > 0.0001d)
                {
                    if (Math.Abs(PreviousSum) < 0.0001d)
                    {
                        Change = 1;
                    }
                    else if (Math.Abs(CurrentSum) < 0.0001d)
                    {
                        Change = -1;
                    }
                    else
                    {
                        Change = Math.Round(CurrentSum/PreviousSum - 1, 4);
                    }
                }
            }
        }

        public DType Type { get; set; }

        public string Name { get; set; }

        public double CurrentSum { get; set; }

        public double PreviousSum { get; set; }

        public double Change { get; set; }
    }
}