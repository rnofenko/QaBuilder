using System;
using Qa.System;

namespace Qa.Combines
{
    public class CombinePromt
    {
        private readonly CombineSettings _settings;
        private readonly FileFinder _finder;
        private readonly FileCombiner _combiner;

        public CombinePromt(Settings settings)
        {
            _settings = new CombineSettings(settings);
            _finder = new FileFinder();
            _combiner = new FileCombiner();
        }

        public void Start()
        {
            while (true)
            {
                Lo.NewPage("Join files");
                showSettings(_settings);

                Lo.Wl()
                    .Wl("Select command:")
                    .Wl("1. Combine")
                    .Wl("2. Show files");

                var key = Console.ReadKey().KeyChar;

                if (key == '1')
                {
                    _combiner.Combine(_settings);
                    break;
                }
                if (key == '2')
                {
                    Lo.NewPage("Files for join:");
                    _finder.Find(_settings.WorkingFolder, _settings.FileMask).ForEach(x => Lo.Wl(x));
                    Lo.WaitAnyKey();
                }
            }
        }

        private void showSettings(CombineSettings settings)
        {
            Lo.Wl().Wl("Combine Settings:")
                .Wl($"File mask      = {settings.FileMask}")
                .Wl($"Working folder = {settings.WorkingFolder}");
        }
    }
}