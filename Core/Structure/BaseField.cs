namespace Qa.Core.Structure
{
    public abstract class BaseField
    {
        public FieldDescription Description { get; }

        public string Title => Description.Title ?? Description.Name;

        protected BaseField(FieldDescription description)
        {
            Description = description;
        }
    }
}