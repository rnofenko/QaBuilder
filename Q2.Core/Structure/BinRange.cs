namespace Q2.Core.Structure
{
    public class BinRange : BaseBinRange
    {
        public string From { get; set; }

        public string To { get; set; }

        public NumericBinRange ToNumeric()
        {
            var bin = new NumericBinRange {Name = Name, Hide = Hide};

            double value;
            if (double.TryParse(From, out value))
            {
                bin.From = value;
            }
            if (double.TryParse(To, out value))
            {
                bin.To = value;
            }

            return bin;
        }
    }
}