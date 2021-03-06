using Qa.Core.Structure;

namespace Qa.Core.Format
{
    public class FormattingFile
    {
        public string SourcePath { get; set; }

        public string DestinationPath { get; set; }

        public FormatStructure FormatStructure { get; set; }
    }
}