namespace Q2.Core.Structure
{
    public class FormatFieldDescription
    {
        public FormatFieldDescription()
        {
        }

        public FormatFieldDescription(Field description)
        {
            Name = description.Name;
            From = description.Name;
        }

        public string Name { get; set; }

        public string From { get; set; }
    }
}