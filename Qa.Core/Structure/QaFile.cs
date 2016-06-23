namespace Qa.Core.Structure
{
    public class QaFile
    {
        public string Path { get; set; }

        public QaStructure Structure { get; set; }

        public override string ToString()
        {
            return Structure + " " + Path;
        }
    }
}