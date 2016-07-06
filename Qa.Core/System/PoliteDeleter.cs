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
                Lo.Error(string.Format("File {0} can't be deleted or recreated.", Path.GetFileName(path)));
                Lo.Error(string.Format("Please, close {0} file.", path));
                Lo.Wl("Preass any key to continue.");
                Console.ReadKey();
                Delete(path);
            }
        }
    }
}
