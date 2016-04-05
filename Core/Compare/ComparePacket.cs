using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class ComparePacket
    {
        public FileStructure Structure { get; set; }

        public List<FileInformation> Files { get; set; }

        public List<NumberField> NumberFields { get; set; }

        public List<GroupedField> GroupedFields { get; set; }

        public ComparePacket(IEnumerable<FileInformation> fileInformations, List<FieldPack> fieldPacks, FileStructure structure)
        {
            Structure = structure;
            Files = fileInformations.ToList();
            
            NumberFields = fieldPacks
                .Where(x => !x.IsGrouped)
                .Where(x=> x.Description.Calculation.Type == CalculationType.CountUnique || x.Description.Type == DType.Numeric)
                .Select(x => new NumberField(x))
                .ToList();

            GroupedFields = fieldPacks
                .Where(x => x.IsGrouped)
                .Select(x => new GroupedField(x))
                .ToList();
        }
    }
}