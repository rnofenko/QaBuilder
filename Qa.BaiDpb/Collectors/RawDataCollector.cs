using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Core;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.BaiDpb.Collectors
{
    public class RawDataCollector
    {
        private readonly StructureDetector _structureDetector;

        public RawDataCollector()
        {
            _structureDetector = new StructureDetector();
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
                    
                    if ((rows % 50000) == 0)
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

        private void processLine(string line, RawReport report)
        {
            var parts = line.Split(new[] { report.Structure.Delimeter }, StringSplitOptions.None);
            report.RowsCount++;

            for (var i = 0; i < parts.Length; i++)
            {
                var field = report.Fields[i];
                var value = parts[i].Replace(" .", "0");
                if (field.Type == DType.Double)
                {
                    if (value.IsNotEmpty())
                    {
                        field.Sum += double.Parse(value);
                    }
                }
                if (field.Type == DType.Money)
                {
                    if (value.IsNotEmpty())
                    {
                        field.Sum += double.Parse(value);
                    }
                }
                else if (field.Type == DType.Int)
                {
                    if (value.IsNotEmpty())
                    {
                        field.Sum += double.Parse(value);
                    }
                }
            }
        }
    }
}