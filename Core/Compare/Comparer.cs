﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core.Collectors;
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

        public List<ComparePacket> Compare(IEnumerable<RawReport> statistics)
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

        private List<FileInformation> compareFiles(IEnumerable<RawReport> rawReports)
        {
            return rawReports.Select(x => new FileInformation { FileName = Path.GetFileNameWithoutExtension(x.Path) }).ToList();
        }

        private FieldPack getFieldPack(IList<RawReportField> rawFields)
        {
            var field = rawFields.First().Description;
            var pack = new FieldPack(field)
            {
                GroupedNumbers = _valuesComparer.Compare(rawFields.Select(x => x.GroupedNumbers), field),
                Numbers = _valuesComparer.Compare(rawFields.Select(x => x.Number))
            };
            
            return pack;
        }
    }
}