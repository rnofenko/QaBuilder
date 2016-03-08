using System;
using System.IO;

namespace Qa.Core.System
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
                Lo.Wl($"File {Path.GetFileName(path)} can't be deleted or recreated.");
                Lo.Wl($"Please, close {path} file.");
                Lo.Wl($"Preass any key to continue.");
                Console.ReadKey();
                Delete(path);
            }
        }
    }
}
