using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class CompareSettings
    {
        private readonly Settings _settings;

        public string FileMask { get; set; }

        public CompareSettings(Settings settings)
        {
            _settings = settings;
            FileMask = "*.txt";
        }
    }
}
