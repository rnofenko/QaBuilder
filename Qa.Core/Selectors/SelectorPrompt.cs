using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.System;

namespace Qa.Core.Selectors
{
    public class SelectorPrompt
    {
        public int Select(IEnumerable<string> choises, string title)
        {
            var list = choises.ToList();
            if (list.IsEmpty())
            {
                return -1;
            }
            if (list.Count == 1)
            {
                return 0;
            }

            while (true)
            {
                Lo.Wl().Wl(title + ":");
                for (var i = 0; i < list.Count; i++)
                {
                    Lo.Wl(string.Format("{0}. {1}", i + 1, list[i]));
                }
                int index = getIndex(list.Count < 10);
                if (index >= 0 && index < list.Count)
                {
                    return index;
                }
                return -1;
            }
        }

        private int getIndex(bool oneDigit)
        {
            try
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    return -1;
                }
                var stringResult = key.KeyChar.ToString();
                if (!oneDigit)
                {
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        return -1;
                    }
                    stringResult += key.KeyChar.ToString();
                }

                int index;
                if (!int.TryParse(stringResult, out index))
                {
                    return -1;
                }
                index--;
                return index;
            }
            finally
            {
                Lo.Wl();
            }
        }
    }
}
