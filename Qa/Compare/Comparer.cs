using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Collectors;
using Qa.Structure;

namespace Qa.Compare
{
    public class Comparer
    {
        public List<ComparePacket> Compare(List<FileStatistics> statistics)
        {
            var packets = statistics
                .GroupBy(x => x.Structure.Name)
                .Select(compare)
                .ToList();
            return packets;
        }

        private ComparePacket compare(IEnumerable<FileStatistics> statistics)
        {
            var files = statistics.OrderBy(x => x.Path).ToList();
            var first = files.First();
            var packet = new ComparePacket {Strucure = first.Structure};

            FileStatistics previous = null;
            foreach (var file in files)
            {
                packet.Files.Add(compare(file, previous));
                previous = file;
            }

            return packet;
        }

        private CompareResult compare(FileStatistics current, FileStatistics previous)
        {
            var result = new CompareResult
            {
                RowsCount = new CompareNumber(current.RowsCount, previous?.RowsCount),
                FileName = Path.GetFileNameWithoutExtension(current.Path)
            };
            for (var i = 0; i < current.Fields.Count; i++)
            {
                var fieldCurrent = current.Fields[i];
                StatisticsField fieldPrev = null;
                if (previous != null)
                {
                    fieldPrev = previous.Fields[i];
                }

                if (fieldCurrent.Type == DType.Float || fieldCurrent.Type == DType.Int || fieldCurrent.Type == DType.Money)
                {
                    result.Fields.Add(new CompareNumberField(fieldCurrent, fieldPrev));
                }
            }
            return result;
        }
    }
}