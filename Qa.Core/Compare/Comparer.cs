using System.Collections.Generic;
using System.IO;
using System.Linq;
using Q2.Core.Compare;
using Q2.Core.Parsers;
using Q2.Core.Structure;
using Qa.Core.Parsers;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class Comparer
    {
        private readonly ValuesComparer _valuesComparer;

        public Comparer()
        {
            _valuesComparer = new ValuesComparer();
        }

        public List<ComparePacket> Compare(IEnumerable<ParsedFile> statistics)
        {
            var packets = statistics
                .GroupBy(x => x.Structure.Name)
                .Select(compare)
                .ToList();
            return packets;
        }

        public List<ComparePacket> Compare(List<ParsedBatch> batches)
        {
            var packets = new List<ComparePacket>();
            for (var i = 0; i < batches.First().Files.Count; i++)
            {
                var index = i;
                var files = batches.Select(x => x.Files[index]).ToList();
                var packet = compare(files);
                packet.SplitValue = files.First().SplitValue;
                packets.Add(packet);
            }
            return packets;
        }

        private ComparePacket compare(IEnumerable<ParsedFile> rawReports)
        {
            var reports = rawReports.OrderBy(x => x.Path).ToList();
            var first = reports.First();

            var fieldPacks = new List<FieldPack>();
            for (var i = 0; i < first.Fields.Count; i++)
            {
                var rawFields = reports.Select(x => x.Fields[i]).ToList();
                fieldPacks.Add(getFieldPack(rawFields));
            }
            var fileInformations = compareFiles(reports);

            return new ComparePacket(fileInformations, fieldPacks, first.Structure);
        }

        private List<FileInformation> compareFiles(IEnumerable<ParsedFile> rawReports)
        {
            var i = 0;
            return rawReports.Select(x => new FileInformation { FileName = Path.GetFileNameWithoutExtension(x.Path), Index = i++ }).ToList();
        }

        private FieldPack getFieldPack(IList<CalculatedField> rawFields)
        {
            var field = rawFields.First().Field;
            var pack = new FieldPack(field)
            {
                GroupedNumbers = _valuesComparer.Compare(rawFields.Select(x => x.GroupedNumbers), field),
                Numbers = _valuesComparer.Compare(rawFields.Select(x => x.Number))
            };
            
            return pack;
        }
    }
}