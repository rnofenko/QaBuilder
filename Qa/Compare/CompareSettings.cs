using System.Collections.Generic;
using Qa.Structure;
using Qa.System;

namespace Qa.Compare
{
    public class CompareSettings
    {
        private readonly Settings _settings;

        public string FileMask { get; set; }

        public bool ShowNotParsedFiles { get; set; }

        public string WorkingFolder => _settings.WorkingFolder;

        public List<FileStructure> FileStructures => _settings.FileStructures;

        public CompareSettings(Settings settings)
        {
            _settings = settings;
            FileMask = "*.txt";
        }
    }
}
