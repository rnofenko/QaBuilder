using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private ComparePacket compare(IEnumerable<ParsedFile> filesForCompare)
        {
            var files = filesForCompare.OrderBy(x => x.Path).ToList();
            var first = files.First();

            var fieldPacks = new List<FieldPack>();
            for (var i = 0; i < first.Fields.Count; i++)
            {
                var fields = files.Select(x => x.Fields[i]).ToList();
                fieldPacks.Add(getFieldPack(fields));
            }

            return new ComparePacket(getFileInfo(files), fieldPacks, first.Structure);
        }

        private List<FileInformation> getFileInfo(IEnumerable<ParsedFile> files)
        {
            var i = 0;
            return files.Select(x => new FileInformation { FileName = Path.GetFileNameWithoutExtension(x.Path), Index = i++ }).ToList();
        }

        private FieldPack getFieldPack(IList<CalculatedField> fields)
        {
            var qa = fields.First().Field;
            var pack = new FieldPack(qa)
            {
                GroupedNumbers = _valuesComparer.Compare(fields.Select(x => x.GroupedNumbers), qa),
                Numbers = _valuesComparer.Compare(fields.Select(x => x.Number))
            };
            
            return pack;
        }
    }
}