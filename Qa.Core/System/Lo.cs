using System;
using System.Diagnostics;

namespace Qa.Core.System
{
    public static class Lo
    {
        private static readonly Stopwatch _watch = new Stopwatch();
        public static Stopwatch Watch
        {
            get { return _watch; }
        }

        public static void ShowFileProcessingProgress(int rowNumber)
        {
            if (rowNumber%20000 != 0)
            {
                return;
            }

            if (rowNumber%1000000 == 0)
            {
                Wl().W(string.Format("Processed {0,2}m Time:{1:mm:ss.fff} ", rowNumber/1000000,
                    new DateTime().AddMilliseconds(_watch.ElapsedMilliseconds)));
            }
            else
            {
                W(".");
            }
        }

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

        public static Logger Ask(string message)
        {
            return new Logger().Ask(message);
        }

        public static Logger Error(string message)
        {
            return new Logger().Error(message);
        }

        public static Logger W(string message, ConsoleColor color)
        {
            return new Logger().W(message, color);
        }

        public static Logger WaitAnyKey()
        {
            return new Logger().WaitAnyKey();
        }
    }
}