namespace Qa.Core.Structure
{
    public abstract class BaseField
    {
        public FieldDescription Description { get; private set; }

        public string Title
        {
            get { return Description.Title ?? Description.Name; }
        }

        protected BaseField(FieldDescription description)
        {
            Description = description;
        }
    }
}