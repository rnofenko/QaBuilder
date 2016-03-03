﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Extensions;
using Qa.Structure;
using Qa.System;

namespace Qa.Collectors
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

            report.Summary = new RawSubReport(detected.Structure.Fields);
            report.SubReportIndex = report.Summary.Fields.FindIndex(f => f.Name == detected.Structure.SubReportBy);

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
            summarize(report);
            print(report, settings);
            return report;
        }

        private void summarize(RawReport file)
        {
            var summary = file.Summary;
            foreach (var subReport in file.SubReports.Values)
            {
                summary.RowsCount += subReport.RowsCount;
                for (var i = 0; i < summary.Fields.Count; i++)
                {
                    summary.Fields[i].Sum += subReport.Fields[i].Sum;
                }
            }
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

            for (var i = 0; i < parts.Length; i++)
            {
                var field = subReport.Fields[i];
                var value = parts[i];
                if (field.Type == DType.Float)
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