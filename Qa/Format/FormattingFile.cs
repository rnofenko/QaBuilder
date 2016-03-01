using Qa.Structure;

namespace Qa.Format
{
    public class FormattingFile
    {
        public string SourcePath { get; set; }

        public string DestinationPath { get; set; }

        public FileStructure Structure { get; set; }
    }
}