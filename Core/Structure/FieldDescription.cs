namespace Qa.Core.Structure
{
    public class FieldDescription
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public bool CountUniqueValues { get; set; }

        public bool SelectUniqueValues { get; set; }

        public DType Type { get; set; }

        public SortType Sort { get; set; }
    }
}