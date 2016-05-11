using System;
using Q2.Bai.Pulse.Sb.Excel;
using Q2.Core.Combines;
using Q2.Core.Format;
using Q2.Core.Structure;
using Q2.Core.System;

namespace Q2.Bai.Pulse.Sb
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
                Console.Clear();
                Lo.NewPage("Santander")
                    .Wl()
                    .Wl(string.Format("Current folder is {0}", settings.WorkingFolder))
                    .Wl()
                    .Wl("Select command:")
                    .Wl("1. Format")
                    .Wl("2. Create QA report")
                    .Wl("3. Combine files");
                var key = Console.ReadKey();
                if (key.KeyChar == '1')
                {
                    new FormatPrompt(settings).Start();
                }
                else if (key.KeyChar == '2')
                {
                    new PulseQaPrompt(settings, new PulseExporter()).Start();
                }
                else if (key.KeyChar == '3')
                {
                    new CombinePromt(settings).Start();
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}