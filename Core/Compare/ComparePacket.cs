using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class ComparePacket
    {
        public FileStructure Structure { get; set; }

        public List<CompareReport> Reports { get; set; }

        public List<UniqueValuesField> UniqueFields { get; set; }

        public List<UniqueCountField> UniqueCounts { get; set; }

        public List<GroupedSumField> GroupedSums { get; set; }

        public ComparePacket(IList<FileInformation> fileInformations, List<FieldPack> fieldPacks, FileStructure structure)
        {
            Structure = structure;
            Reports = new List<CompareReport>();

            for (var i = 0; i < fileInformations.Count; i++)
            {
                var report = new CompareReport
                {
                    RowsCount = fileInformations[i].RowsCount,
                    FileName = fileInformations[i].FileName,
                    Numbers = fieldPacks.Where(x => x.Type == DType.Number).Select(x => x.GetNumberField(i)).ToList(),
                };
                Reports.Add(report);
            }

            UniqueFields = fieldPacks
                .Where(UniqueValuesField.IsConvertable)
                .Select(x => new UniqueValuesField(x))
                .ToList();

            UniqueCounts = fieldPacks
                .Where(UniqueCountField.IsConvertable)
                .Select(x => new UniqueCountField(x))
                .ToList();

            GroupedSums = fieldPacks
                .Where(GroupedSumField.IsConvertable)
                .Select(x => new GroupedSumField(x))
                .ToList();
        }
    }
}