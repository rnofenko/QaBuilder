using System;
using Q2.Argus.Cd.Fields;
using Q2.Core.Excel;
using Q2.Core.Format;
using Q2.Core.Qa;
using Q2.Core.Structure;
using Q2.Core.System;

namespace Q2.Argus.Cd
{
    public class Prompt
    {
        private static SettingsProvider _settingsProvider;

        public void Run()
        {
            _settingsProvider = new SettingsProvider();
            var settings = _settingsProvider.Load();
            ServiceLocator.CalculationFieldFactory = new CustomCalculationFieldFactory();

            while (true)
            {
                Console.Clear();
                Lo.NewPage("Santander")
                    .Wl()
                    .Wl(string.Format("Current folder is {0}", settings.WorkingFolder))
                    .Wl()
                    .Wl("Select command:")
                    .Wl("1. Format")
                    .Wl("2. Create QA report");
                var key = Console.ReadKey();
                if (key.KeyChar == '1')
                {
                    new FormatPrompt(settings).Start();
                }
                else if (key.KeyChar == '2')
                {
                    new QaPrompt(settings, new Exporter(new CommonPage(new CommonPageSettings {Freeze = false}))).Start();
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}