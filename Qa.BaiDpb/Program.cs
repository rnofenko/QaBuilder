﻿using System;
using System.IO;
using Qa.BaiDpb.Compare;
using Qa.Core.Combines;
using Qa.Core.Format;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.System;

namespace Qa.BaiDpb
{
    class Program
    {
        private static SettingsProvider _settingsProvider;

        static void Main(string[] args)
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
                    .Wl("3. Combine files")
                    .Wl("4. Set working folder");
                if (Fast.Qa)
                {
                    new ComparePrompt(settings).Start();
                    break;
                }
                else
                {
                    var key = Console.ReadKey().KeyChar;
                    if (key == '1')
                    {
                        new FormatPrompt(settings).Start();
                    }
                    else if (key == '2')
                    {
                        new ComparePrompt(settings).Start();
                    }
                    else if (key == '3')
                    {
                        new CombinePromt(settings).Start();
                    }
                    else if (key == '4')
                    {
                        SetWorkingFolder(settings);
                    }
                }
            }
        }

        private static void SetWorkingFolder(Settings settings)
        {
            Lo.Wl().Wl($"Current folder is {settings.WorkingFolder}")
                .W("Input new folder:");
            var folder = Console.ReadLine() ?? "";
            if (Directory.Exists(folder))
            {
                settings.WorkingFolder = folder;
                _settingsProvider.Save(settings);
            }
            else
            {
                Lo.Wl("Inputted folder doesn't exist.");
            }
        }
    }
}