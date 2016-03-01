using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Extensions;
using Qa.Structure;
using Qa.System;

namespace Qa.Collectors
{
    public class FileStatisticsCollector
    {
        private readonly StructureDetector _structureDetector;

        public FileStatisticsCollector()
        {
            _structureDetector = new StructureDetector();
        }

        public List<FileStatistics> Collect(List<string> files, CollectionSettings settings)
        {
            var statisticsByFiles = files
                .Select(x => new FileStatisticsCollector().Collect(x, settings))
                .Where(x => x.Error.IsEmpty())
                .OrderBy(x => x.Structure.Name)
                .ToList();
            return statisticsByFiles;
        }

        public FileStatistics Collect(string filepath, CollectionSettings settings)
        {
            var detected = _structureDetector.Detect(filepath,
                new StructureDetectSettings { FileStructures = settings.FileStructures });

            var stats = new FileStatistics
            {
                Path = filepath,
                Error = detected.Error,
                Structure = detected.Structure,
                FieldsCount = detected.FieldsCount
            };

            if (stats.Error.IsNotEmpty())
            {
                return stats;
            }

            stats.Fields = detected.Structure.Fields.Select(x => new StatisticsField(x)).ToList();

            using (var  stream = new StreamReader(filepath))
            {
                for (var i = 0; i < stats.Structure.RowsInHeader; i++)
                {
                    stream.ReadLine();
                }

                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    processLine(line, stats);
                    stats.RowsCount++;

                    if ((stats.RowsCount%10000) == 0)
                    {
                        Lo.Wl($"Processed {stats.RowsCount}");
                    }
                }
            }
            print(stats, settings);
            return stats;
        }

        private void print(FileStatistics stats, CollectionSettings settings)
        {
            if (stats.Error.IsEmpty() || !settings.ShowError)
            {
                return;
            }

            Lo.Wl()
                .Wl(stats.Path)
                .Wl("ERROR      : " + stats.Error, stats.Error.IsNotEmpty())
                .Wl($"Structure  : {stats.Structure?.Name}", stats.Structure != null)
                .Wl($"RowsCount  : {stats.RowsCount}")
                .Wl($"FieldsCount: {stats.FieldsCount}");
        }

        private void processLine(string line, FileStatistics stats)
        {
            var parts = line.Split(new[] { stats.Structure.Delimeter }, StringSplitOptions.None);
            for (var i = 0; i < parts.Length; i++)
            {
                var field = stats.Fields[i];
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