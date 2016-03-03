using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.BAI_DPB.Collectors
{
    public class CollectionSettings
    {
        public List<FileStructure> FileStructures { get; set; }

        public bool ShowError { get; set; }
    }
}