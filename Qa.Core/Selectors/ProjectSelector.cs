using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Qa.Core.Selectors
{
    public class ProjectSelector
    {
        private readonly SelectorPrompt _selector;

        public ProjectSelector()
        {
            _selector = new SelectorPrompt();
        }

        public string Select(string binFolder)
        {
            var files = Directory.GetFiles(binFolder, "*.json").Select(Path.GetFileNameWithoutExtension).ToList();
            if (files.IsEmpty())
            {
                throw new WarningException("There is not any json project files.");
            }

            var index = _selector.Select(files, "Select project");
            if (index < 0)
            {
                throw new WarningException("QaBuilder doesn't work without project json file.");
            }
            return files[index];
        }
    }
}