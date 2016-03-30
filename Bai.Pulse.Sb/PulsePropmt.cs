using System;
using Qa.Bai.Pulse.Sb.Compare;
using Qa.Core.Combines;
using Qa.Core.Format;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Bai.Pulse.Sb
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
                    .Wl($"Current folder is {settings.WorkingFolder}")
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
                    new PulseComparePrompt(settings).Start();
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