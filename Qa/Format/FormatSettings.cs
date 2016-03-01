using System.Collections.Generic;
using Qa.Structure;
using Qa.System;

namespace Qa.Format
{
    public class FormatSettings
    {
        private readonly Settings _settings;

        public string SourceFileMask { get; set; }

        public string SourceDelimeter { get; set; }

        public string DestinationDelimeter { get; set; }

        public string DestinationFileExtension { get; set; }

        public int SkipRows { get; set; }

        public string WorkingFolder => _settings.WorkingFolder;

        public List<FileStructure> FileStructures => _settings.FileStructures;

        public FormatSettings(Settings settings)
        {
            _settings = settings;
            SourceDelimeter = ",";
            SourceFileMask = "*.csv";
            DestinationDelimeter = "|";
            DestinationFileExtension = "txt";
            SkipRows = 1;
        }
    }
}