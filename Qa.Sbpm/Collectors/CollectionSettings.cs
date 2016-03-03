using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Sbpm.Collectors
{
    public class CollectionSettings
    {
        public List<FileStructure> FileStructures { get; set; }

        public bool ShowError { get; set; }
    }
}