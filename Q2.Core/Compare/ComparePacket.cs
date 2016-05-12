using System.Collections.Generic;
using System.Linq;
using Q2.Core.Structure;

namespace Q2.Core.Compare
{
    public class ComparePacket
    {
        public QaStructure Structure { get; set; }

        public List<FileInformation> Files { get; set; }

        public List<NumberField> NumberFields { get; set; }

        public List<GroupedField> GroupedFields { get; set; }

        public string SplitValue { get; set; }

        public ComparePacket(IEnumerable<FileInformation> fileInformations, List<FieldPack> fieldPacks, QaStructure structure)
        {
            Structure = structure;
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