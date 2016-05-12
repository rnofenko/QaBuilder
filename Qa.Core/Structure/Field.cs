using Q2.Core.Structure;

namespace Qa.Core.Structure
{
    public class Field
    {
        public string Name { get; set; }

        public DType Type { get; set; }

        public NumberFormat NumberFormat { get; set; }

        public string DateFormat { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}