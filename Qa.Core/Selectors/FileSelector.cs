using System.IO;
using System.Linq;
using Qa.Core.System;

namespace Qa.Core.Selectors
{
    public class FileSelector
    {
        private readonly SelectorPrompt _selector;
        private readonly PathFinder _pathFinder;

        public FileSelector()
        {
            _selector = new SelectorPrompt();
            _pathFinder = new PathFinder();
        }

        public string SelectAnyFile(string folder)
        {
            var filePaths = _pathFinder.Find(folder);
            var fileIndex = _selector.Select(filePaths.Select(Path.GetFileName), "Select file");
            if (fileIndex < 0)
            {
                return null;
            }
            return filePaths[fileIndex];
        }
    }
}
