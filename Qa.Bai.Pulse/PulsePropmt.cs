using System;
using Qa.Bai.Pulse.Excel;
using Qa.Core.Combines;
using Qa.Core.Excel;
using Qa.Core.Format;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Bai.Pulse
{
    public class PulsePropmt
    {
        private SettingsProvider _settingsProvider;

        public void Run()
        {
            _settingsProvider = new SettingsProvider();
            var settings = _settingsProvider.Load();

            while (true)
            {
                Lo.Wl().Wl().Wl(string.Format("..................... Santander - {0} .........................", settings.Project))
                    .Wl(string.Format("Current folder is {0}", settings.WorkingFolder))
                    .Wl()
                    .Wl("Select command:")
                    .Wl("1. Format")
                    .Wl("2. Create QA report")
                    .Wl("3. Combine files")
                    .Wl("4. xls to csv");
                var key = Console.ReadKey();
                if (key.KeyChar == '1')
                {
                    new FormatPrompt().Start(settings);
                }
                else if (key.KeyChar == '2')
                {
                    new PulseQaPrompt(settings, new PulseExporter()).Start();
                }
                else if (key.KeyChar == '3')
                {
                    new CombinePromt(settings).Start();
                }
                else if (key.KeyChar == '4')
                {
                    new XlsToCsvConverter().ConvertAll(settings.WorkingFolder);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}