namespace Qa.Structure
{
    public class StructureDetectResut
    {
        public string Error { get; set; }

        public int FieldsCount { get; set; }

        public FileStructure Structure { get; set; }

        public string FilePath { get; set; }

        public string Line { get; set; }
    }
}