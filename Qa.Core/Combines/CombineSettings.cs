using Qa.Core.Structure;

namespace Qa.Core.Combines
{
    public class CombineSettings
    {
        private readonly Settings _settings;

        public string FileMask { get; set; }

        public string WorkingFolder => _settings.WorkingFolder;

        public CombineSettings(Settings settings)
        {
            _settings = settings;
            FileMask = "*.txt";
        }
    }
}