using System;

namespace Qa.Core.System
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

        public static Logger Wl(int count)
        {
            var logger = new Logger();
            for (int i = 0; i < count; i++)
            {
                logger.Wl();
            }
            return logger;
        }

        public static Logger Wl(string message, ConsoleColor color)
        {
            return new Logger().Wl(message, color);
        }

        public static Logger WaitAnyKey()
        {
            return new Logger().WaitAnyKey();
        }
    }
}