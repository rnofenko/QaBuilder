using System;

namespace Qa.System
{
    public class Logger
    {
        public Logger W(string message = "", bool show = true)
        {
            if (show)
            {
                Console.Write(message);
            }
            return this;
        }

        public Logger Wl(string message = "", bool show = true)
        {
            if (show)
            {
                Console.WriteLine(message);
            }
            return this;
        }

        public Logger WaitAnyKey()
        {
            Wl("Press any key to continue");
            Console.ReadKey();
            return this;
        }
    }
}