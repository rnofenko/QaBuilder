using System;

namespace Qa.Core.System
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

        public Logger Ask(string message)
        {
            Wl(message, ConsoleColor.Cyan);
            return this;
        }

        public Logger Error(string message)
        {
            Wl(message, ConsoleColor.Red);
            return this;
        }

        public Logger Warning(string message)
        {
            Wl(message, ConsoleColor.Yellow);
            return this;
        }

        public Logger Highlight(string message)
        {
            Wl(message, ConsoleColor.Yellow);
            return this;
        }

        public Logger Success(string message)
        {
            Wl(message, ConsoleColor.Green);
            return this;
        }

        public Logger Wl(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            return this;
        }

        public Logger W(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
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