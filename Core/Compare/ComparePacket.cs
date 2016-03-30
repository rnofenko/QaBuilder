using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class ComparePacket
    {
        public FileStructure Structure { get; set; }

        public List<CompareReport> Reports { get; set; }

        public List<NumberField> NumberFields { get; set; }

        public List<GroupedField> GroupedFields { get; set; }

        public ComparePacket(IList<FileInformation> fileInformations, List<FieldPack> fieldPacks, FileStructure structure)
        {
            Structure = structure;
            Reports = new List<CompareReport>();

            for (var i = 0; i < fileInformations.Count; i++)
            {
                var report = new CompareReport
                {
                    Index = i,
                    FileName = fileInformations[i].FileName,
                    Numbers = fieldPacks.Where(x => x.Type == DType.Numeric && !x.IsGrouped).Select(x => x.GetNumberField(i)).ToList(),
                };
                Reports.Add(report);
            }

            NumberFields = fieldPacks
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