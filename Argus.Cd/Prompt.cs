using System;
using Qa.Core.Excel;
using Qa.Core.Format;
using Qa.Core.Qa;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Argus.Cd
{
    public class Prompt
    {
        private static SettingsProvider _settingsProvider;

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
                    .Wl("2. Create QA report");
                var key = Console.ReadKey().KeyChar;
                if (key == '1')
                {
                    new FormatPrompt(settings).Start();
                }
                else if (key == '2')
                {
                    new QaPrompt(settings, new Exporter(new CommonPage())).Start();
                }
            }
        }
    }
}