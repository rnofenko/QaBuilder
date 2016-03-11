using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Novantas.SaleScape.Dr.Collectors;
using Qa.Core.Compare;
using Qa.Core.Structure;

namespace Qa.Novantas.SaleScape.Dr.Compare
{
    public class Comparer
    {
        private readonly UniqueValuesComparer _uniqueValuesComparer;

        public Comparer()
        {
            _uniqueValuesComparer = new UniqueValuesComparer();
        }

        public List<ComparePacket> Compare(List<RawReport> statistics)
        {
            var packets = statistics
                .GroupBy(x => x.Structure.Name)
                .Select(compare)
                .ToList();
            return packets;
        }

        private ComparePacket compare(IEnumerable<RawReport> rawReports)
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

        private List<FileInformation> compareFiles(IList<RawReport> rawReports)
        {
            RawReport previous = null;
            var reports = new List<FileInformation>();
            foreach (var current in rawReports)
            {
                reports.Add(new FileInformation
                {
                    FileName = Path.GetFileNameWithoutExtension(current.Path),
                    RowsCount = new CompareNumber(current.RowsCount, previous?.RowsCount)
                });
                previous = current;
            }
            return reports;
        }

        private FieldPack getFieldPack(IList<RawReportField> rawFields)
        {
            var field = rawFields.First().Description;
            var pack = new FieldPack(field);

            if (field.SelectUniqueValues)
            {
                pack.UniqueValueSet = _uniqueValuesComparer.Compare(rawFields);
            }

            if (field.CountUniqueValues)
            {
                pack.Numbers = _uniqueValuesComparer.CompareCounts(rawFields);
            }

            if (field.Type == DType.Double || field.Type == DType.Int || field.Type == DType.Money)
            {
                pack.Numbers = compareNumbers(rawFields);
            }

            return pack;
        }

        private List<CompareNumber> compareNumbers(IList<RawReportField> rawFields)
        {
            RawReportField previous = null;
            var numbers = new List<CompareNumber>();
            foreach (var current in rawFields)
            {
                numbers.Add(new CompareNumber(current.Sum, previous?.Sum));
                previous = current;
            }
            return numbers;
        }
    }
}