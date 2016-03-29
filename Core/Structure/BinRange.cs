namespace Qa.Core.Structure
{
    public class BinRange
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Name { get; set; }

        public NumericBinRange ToNumeric()
        {
            var bin = new NumericBinRange {Name = Name};

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