using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class ComparePacket
    {
        public CompareFilesMethod CompareMethod { get; private set; }

        public List<FileInformation> Files { get; set; }

        public List<NumberField> NumberFields { get; set; }

        public List<GroupedField> GroupedFields { get; set; }

        public string SplitValue { get; set; }

        public ComparePacket(IEnumerable<FileInformation> fileInformations, List<FieldPack> fieldPacks, CompareFilesMethod compareMethod)
        {
            CompareMethod = compareMethod;
            Files = fileInformations.ToList();
            
            NumberFields = fieldPacks
                .Where(x => !x.Field.Group)
                .Select(x => new NumberField(x))
                .ToList();

            GroupedFields = fieldPacks
                .Where(x => x.Field.Group)
                .Select(x => new GroupedField(x))
                .ToList();
        }
    }
}