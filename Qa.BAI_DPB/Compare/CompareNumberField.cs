using System;
using Qa.Core.Structure;
using Qa.Structure;

namespace Qa.BAI_DPB.Compare
{
    public class CompareNumberField
    {
        private readonly FieldDescription _description;

        public DType Type => _description.Type;

        public string Name => _description.Name;

        public string Title => _description.Title ?? _description.Name;

        public double CurrentSum { get; set; }

        public double PreviousSum { get; set; }

        public double Change { get; set; }

        public CompareNumberField(RawReportField current, RawReportField previous)
        {
            _description = current.Description;
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
    }
}