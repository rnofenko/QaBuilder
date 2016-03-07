using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core;
using Qa.Core.Collectors;
using Qa.Core.Structure;
using Qa.Core.System;
using Qa.Sbpm;
using Qa.Sbpm.Collectors;

namespace Qa.Bai.Sbp.Collectors
{
    public class RawDataCollector
    {
        private readonly StructureDetector _structureDetector;
        private readonly ValueParser _valueParser;

        public RawDataCollector()
        {
            _structureDetector = new StructureDetector();
            _valueParser = new ValueParser();
        }

        public List<RawReport> Collect(IEnumerable<string> files, CollectionSettings settings)
        {
            var statisticsByFiles = files
                .Select(x => new RawDataCollector().collect(x, settings))
                .Where(x => x.Error.IsEmpty())
                .OrderBy(x => x.Structure.Name)
                .ToList();
            return statisticsByFiles;
        }

        private RawReport collect(string filepath, CollectionSettings settings)
        {
            var detected = _structureDetector.Detect(filepath,
                new StructureDetectSettings { FileStructures = settings.FileStructures });

            var report = new RawReport(detected.Structure.Fields)
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
            
            report.SubReportIndex = report.Fields.FindIndex(f => f.Name == QaSettings.Field.State);

            using (var  stream = new StreamReader(filepath))
            {
                for (var i = 0; i < report.Structure.RowsInHeader; i++)
                {
                    stream.ReadLine();
                }

                string line;
                var rows=0;
                while ((line = stream.ReadLine()) != null)
                {
                    processLine(line, report);
                    rows++;
                    
                    if ((rows % 25000) == 0)
                    {
                        Lo.Wl($"Processed {rows}");
                    }
                }
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

        private void processLine(string line, RawReport stats)
        {
            var parts = line.Split(new[] { stats.Structure.Delimeter }, StringSplitOptions.None);
            var subReport = stats.GetSubReport(parts);
            subReport.RowsCount++;
            _valueParser.Parse(parts, subReport.Fields);
        }
    }
}