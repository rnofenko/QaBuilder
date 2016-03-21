using System;
using Qa.Core.Combines;
using Qa.Core.Format;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.ImportFiles
{
    public class Propmt
    {
        private SettingsProvider _settingsProvider;

        public void Run()
        {
            _settingsProvider = new SettingsProvider();
            var settings = _settingsProvider.Load();
            var combiner = new FileCombiner();

            while (true)
            {
                Console.Clear();
                Lo.NewPage("Santander")
                    .Wl()
                    .Wl($"Current folder is {settings.WorkingFolder}")
                    .Wl()
                    .Wl("Select command:")
                    .Wl("1. Format files")
                    .Wl("2. Combine by mask")
                    .Wl("3. Add date");
                var key = Console.ReadKey().KeyChar;
                if (key == '1')
                {
                    new FormatPrompt(settings).Start();
                }
                if (key == '2')
                {
                    Lo.W("Input mask:");
                    var mask = Console.ReadLine();
                    combiner.Combine(new CombineSettings(settings) { HeaderRowsCount = 1, FileMask = mask });
                    Lo.Wl("Combine was finished.");
                    Console.ReadLine();
                }
                if (key == '3')
                {
                    new DateAdder().Add(settings);
                }
            }
        }
    }
}
