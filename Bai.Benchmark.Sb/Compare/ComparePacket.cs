using System.Collections.Generic;
using System.Linq;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Bai.Benchmark.Sb.Compare
{
    public class ComparePacket
    {
        public FileStructure Structure { get; set; }

        public List<CompareReport> Reports { get; set; }

        public List<FieldPack> UniqueFields { get; set; }

        public List<FieldPack> UniqueCounts { get; set; }

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

            UniqueFields = fieldPacks.Where(x => x.SelectUniqueValues).ToList();
            UniqueCounts = fieldPacks.Where(x => x.CountUniqueValues).ToList();
        }
    }
}