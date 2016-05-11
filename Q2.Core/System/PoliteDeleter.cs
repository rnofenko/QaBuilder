using System;
using System.IO;

namespace Q2.Core.System
{
    public class PoliteDeleter
    {
        public void Delete(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                File.Delete(path);
            }
            catch (IOException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Lo.Wl("File {Path.GetFileName(path)} can't be deleted or recreated.");
                Lo.Wl("Please, close {path} file.");
                Lo.Wl("Preass any key to continue.");
                Console.ResetColor();
                Console.ReadKey();
                Delete(path);
            }
        }
    }
}
