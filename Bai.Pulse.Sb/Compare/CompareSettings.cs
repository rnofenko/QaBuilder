using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Bai.Pulse.Sb.Compare
{
    public class CompareSettings
    {
        private readonly Settings _settings;

        public string FileMask { get; set; }

        public bool ShowNotParsedFiles { get; set; }

        public string WorkingFolder
        {
            get { return _settings.WorkingFolder; }
        }

        public List<FileStructure> FileStructures
        {
            get { return _settings.FileStructures; }
        }

        public CompareSettings(Settings settings)
        {
            _settings = settings;
            FileMask = "*.txt";
        }
    }
}
