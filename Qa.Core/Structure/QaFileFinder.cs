using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Compare;
using Qa.Core.Parsers;
using Qa.Core.System;

namespace Qa.Core.Structure
{
    public class QaFileFinder
    {
        private readonly PathFinder _pathFinder;
        private readonly StructureDetector _detector;
        private IEnumerable<QaStructure> _qaStructures;
        private readonly FileDateExtractor _dateExtractor;

        public QaFileFinder()
        {
            _pathFinder = new PathFinder();
            _detector = new StructureDetector();
            _dateExtractor = new FileDateExtractor();
        }

        public List<QaFile> Find(Settings settings)
        {
            _qaStructures = settings.FileStructures.Select(x => x.Qa);

            var files = _pathFinder
                .Find(settings.WorkingFolder)
                .Select(getParseArgs)
                .Where(x => x != null)
                .ToList();

            files = excludeUnusedFilesForFourMonthsComparing(files);

            return files;
        }

        private List<QaFile> excludeUnusedFilesForFourMonthsComparing(List<QaFile> files)
        {
            var ready = files.Where(x => x.Structure.CompareFilesMethod != CompareFilesMethod.FourMonths).ToList();

            var fourFilesGrouppedByStructure = files
                .Where(x => x.Structure.CompareFilesMethod == CompareFilesMethod.FourMonths)
                .GroupBy(x => x.Structure.Name);

            foreach (var filesForStructure in fourFilesGrouppedByStructure)
            {
                var filtered = getFourFiles(filesForStructure.ToList());
                ready.AddRange(filtered);
            }

            return ready;
        }

        private List<QaFile> getFourFiles(List<QaFile> list)
        {
            //filter out wrong files
            list = list.Select(x =>
            {
                if (_dateExtractor.Extract(x.Path) == null)
                {
                    Lo.Wl("File '" + x + "' doesn't have date in file name");
                    return null;
                }
                return x;
            }).Where(x => x != null).ToList();

            var maxDate = list.Max(x => _dateExtractor.Extract(x.Path));
            if (maxDate == null)
            {
                return list;
            }

            var currentMonth = list.First(x => _dateExtractor.Extract(x.Path) == maxDate.Value);
            var previousMonth = list.FirstOrDefault(x => _dateExtractor.Extract(x.Path) == maxDate.Value.AddMonths(-1));
            var previousYear = list.FirstOrDefault(x => _dateExtractor.Extract(x.Path) == maxDate.Value.AddYears(-1));
            var lastDecember = list.FirstOrDefault(x => _dateExtractor.Extract(x.Path) == new DateTime(maxDate.Value.Year - 1, 12, 1));

            return new[] {currentMonth, previousMonth, lastDecember, previousYear}.Where(x => x != null).ToList();
        }

        private QaFile getParseArgs(string path)
        {
            var structure = _detector.Detect(path, _qaStructures);
            if (structure == null)
            {
                return null;
            }
            return new QaFile { Path = path, Structure = structure };
        }
    }
}
