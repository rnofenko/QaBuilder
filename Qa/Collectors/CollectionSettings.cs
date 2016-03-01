using System.Collections.Generic;
using Qa.Structure;

namespace Qa.Collectors
{
    public class CollectionSettings
    {
        public List<FileStructure> FileStructures { get; set; }

        public bool ShowError { get; set; }
    }
}