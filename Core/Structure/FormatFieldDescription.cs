namespace Qa.Core.Structure
{
    public class FormatFieldDescription
    {
        public FormatFieldDescription()
        {
        }

        public FormatFieldDescription(FieldDescription description)
        {
            Name = description.Name;
            From = description.Name;
        }

        public string Name { get; set; }

        public string From { get; set; }
    }
}