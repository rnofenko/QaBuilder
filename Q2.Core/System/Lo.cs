using System;
using Qa.Core.System;

namespace Q2.Core.System
{
    public static class Lo
    {
        public static Logger W(string message = "")
        {
            return new Logger().W(message);
        }

        public static Logger Wl(string message = "")
        {
            return new Logger().Wl(message);
        }

        public static Logger Wl(string message, ConsoleColor color)
        {
            return new Logger().Wl(message, color);
        }

        public static Logger WaitAnyKey()
        {
            return new Logger().WaitAnyKey();
        }

        public static Logger NewPage(string message = "")
        {
            Console.Clear();
            return new Logger().Wl(message);
        }
    }
}