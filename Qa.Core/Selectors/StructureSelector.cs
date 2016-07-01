using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Selectors
{
    public class StructureSelector
    {
        private readonly SelectorPrompt _selector;

        public StructureSelector()
        {
            _selector = new SelectorPrompt();
        }

        public FileStructure Select(Settings settings)
        {
            var index = _selector.Select(settings.FileStructures.Select(x => x.Qa.Name), "Select structure");
            if (index < 0)
            {
                return null;
            }
            return settings.FileStructures[index];
        }
    }
}