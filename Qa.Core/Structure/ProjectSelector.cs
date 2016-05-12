using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Q2.Core.Extensions;
using Q2.Core.System;

namespace Qa.Core.Structure
{
    public class ProjectSelector
    {
        public string Select(string binFolder)
        {
            var files = Directory.GetFiles(binFolder, "*.json").Select(Path.GetFileNameWithoutExtension).ToList();
            if (files.IsEmpty())
            {
                throw new WarningException("There is not any json project files.");
            }

            if (files.Count == 1)
            {
                return files.First();
            }

            while (true)
            {
                Console.Clear();
                Lo.NewPage("Select project:");
                for (var i = 0; i < files.Count; i++)
                {
                    Lo.Wl($"{i + 1}. {files[i]}");
                }
                var key = Console.ReadKey();
                int index;
                int.TryParse(key.KeyChar.ToString(), out index);
                index--;
                if (index >= 0 && index < files.Count)
                {
                    return files[index];
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    throw new WarningException("QaBuilder doesn't work without project json file.");
                }
            }
        }
    }
}