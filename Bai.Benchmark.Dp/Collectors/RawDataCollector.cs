using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core;
using Qa.Core.Collectors;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Bai.Benchmark.Dp.Collectors
{
    public class RawDataCollector
    {
        private readonly StructureDetector _structureDetector;
        
        public RawDataCollector()
        {
            _structureDetector = new StructureDetector();
        }

        public List<RawReport> CollectReports(IEnumerable<string> files, CollectionSettings settings)
        {
            var statisticsByFiles = files
                .Select(x => new RawDataCollector().collectReport(x, settings))
                .Where(x => x.Error.IsEmpty())
                .OrderBy(x => x.Structure.Name)
                .ToList();
            return statisticsByFiles;
        }

        private RawReport collectReport(string filepath, CollectionSettings settings)
        {
            Lo.Wl($"File: {Path.GetFileNameWithoutExtension(filepath)}");
            var detected = _structureDetector.Detect(filepath,
                new StructureDetectSettings { FileStructures = settings.FileStructures });

            var report = new RawReport
            {
                Path = filepath,
                Error = detected.Error,
                Structure = detected.Structure,
                FieldsCount = detected.FieldsCount
            };
            if (report.Error.IsNotEmpty())
            {
                return report;
            }

            using (var valueParser = new ValueParser(detected.Structure.Fields))
            {
                using (var stream = new StreamReader(filepath))
                {
                    for (var i = 0; i < report.Structure.RowsInHeader; i++)
                    {
                        stream.ReadLine();
                    }

                    string line;
                    while ((line = stream.ReadLine()) != null)
                    {
                        var parts = line.Split(new[] { report.Structure.Delimeter }, StringSplitOptions.None);
                        report.RowsCount++;
                        valueParser.Parse(parts);
                        
                        if ((report.RowsCount % 500000) == 0)
                        {
                            Lo.Wl($"Processed {report.RowsCount}");
                        }
                    }
                }

                report.Fields = valueParser.Fields.Select(x => new RawReportField(x)).ToList();
            }
            
            print(report, settings);
            return report;
        }
        
        private void print(RawReport stats, CollectionSettings settings)
        {
            if (stats.Error.IsEmpty() || !settings.ShowError)
            {
                return;
            }

            Lo.Wl()
                .Wl(stats.Path)
                .Wl("ERROR      : " + stats.Error, stats.Error.IsNotEmpty())
                .Wl($"Structure  : {stats.Structure?.Name}", stats.Structure != null)
                .Wl($"FieldsCount: {stats.FieldsCount}");
        }
    }
}