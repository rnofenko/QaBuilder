namespace Qa.Core.Structure
{
    public class BinRange
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Name { get; set; }

        public NumericBinRange ToNumeric()
        {
            return new NumericBinRange {Name = Name, To = double.Parse(To), From = double.Parse(From)};
        }
    }
}