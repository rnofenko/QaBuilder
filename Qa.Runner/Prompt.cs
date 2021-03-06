﻿using System;
using Qa.Core.Displayers;
using Qa.Core.Editors;
using Qa.Core.Excel;
using Qa.Core.Format;
using Qa.Core.Qa;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.Runner.ArgusCd;

namespace Qa.Runner
{
    public class Prompt
    {
        private static SettingsProvider _settingsProvider;

        public void Run()
        {
            _settingsProvider = new SettingsProvider();
            var settings = _settingsProvider.Load();
            if (settings == null)
            {
                return;
            }

            if (settings.Project == "argusCd")
            {
                ServiceLocator.CalculationFieldFactory = new ArgusCdCalculationFieldFactory();
            }
            
            while (true)
            {
                Lo.Wl().Wl().Wl(string.Format("..................... Santander - {0} .........................", settings.Project))
                    .Wl(string.Format("Current folder is {0}", settings.WorkingFolder))
                    .Wl()
                    .Wl("Select command:")
                    .Wl("1. Format")
                    .Wl("2. Create QA report")
                    .Wl("3. Show data")
                    .Wl("4. Edit file");
                var key = Console.ReadKey();
                Lo.Wl();
                if (key.KeyChar == '1')
                {
                    new FormatPrompt().Start(settings);
                }
                else if (key.KeyChar == '2')
                {
                    new QaPrompt(new Exporter(new CommonPage(new CommonPageSettings {Freeze = false}))).Start(settings);
                }
                else if (key.KeyChar == '3')
                {
                    new DisplayPrompt().Start(settings);
                }
                else if (key.KeyChar == '4')
                {
                    new EditPrompt().Start(settings);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}