using Q2.Core.Structure;

namespace Q2.Core.Combines
{
    public class CombineSettings
    {
        private readonly Settings _settings;

        public string FileMask { get; set; }

        public string WorkingFolder
        {
            get { return _settings.WorkingFolder; }
        }

        public int HeaderRowsCount { get; set; }

        public CombineSettings(Settings settings)
        {
            _settings = settings;
            FileMask = "*.txt";
        }
    }
}