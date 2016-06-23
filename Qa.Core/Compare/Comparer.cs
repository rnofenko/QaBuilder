using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.Parsers;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class Comparer
    {
        private readonly ValueComparerFactory _factory;
        
        public Comparer()
        {
            _factory = new ValueComparerFactory();
        }

        public List<ComparePacket> Compare(List<ParsedBatch> batches, CompareFilesMethod compareFilesMethod)
        {
            var packets = new List<ComparePacket>();
            for (var i = 0; i < batches.First().Files.Count; i++)
            {
                var index = i;
                var files = batches.Select(x => x?.Files[index]).ToList();
                var packet = Compare(files, compareFilesMethod);
                packet.SplitValue = files.First().SplitValue;
                packets.Add(packet);
            }
            return packets;
        }

        public ComparePacket Compare(IEnumerable<ParsedFile> filesForCompare, CompareFilesMethod compareFilesMethod)
        {
            var files = filesForCompare.ToList();
            var first = files.First();
            var valueComparer = _factory.Create(compareFilesMethod);

            var fieldPacks = new List<FieldPack>();
            for (var i = 0; i < first.Fields.Count; i++)
            {
                var fields = files.Select(x => x?.Fields[i]).ToList();
                fieldPacks.Add(getFieldPack(fields, valueComparer));
            }

            return new ComparePacket(getFileInfo(files), fieldPacks, compareFilesMethod);
        }

        private IEnumerable<FileInformation> getFileInfo(IEnumerable<ParsedFile> files)
        {
            var i = 0;
            return files.Select(x => new FileInformation
                {
                    FileName = Path.GetFileNameWithoutExtension(x.Path),
                    Index = i++
                }).ToList();
        }

        private FieldPack getFieldPack(IList<CalculatedField> fields, IValueComparer valueComparer)
        {
            var qa = fields.First().Field;
            var pack = new FieldPack(qa)
            {
                GroupedNumbers = valueComparer.Compare(fields.Select(x => x.GroupedNumbers), qa),
                Numbers = valueComparer.Compare(fields.Select(x => x.Number))
            };

            return pack;
        }
    }
}