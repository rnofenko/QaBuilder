namespace Qa.Core.Structure
{
    public class BinRange
    {
        public string Name { get; set; }

        public bool Hide { get; set; }

        public string UpTo { get; set; }

        public bool SplitToItems { get; set; }

        public bool Last { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}