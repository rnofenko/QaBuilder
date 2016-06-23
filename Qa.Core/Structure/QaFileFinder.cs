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
        private readonly FileDateExtractor _dateExtractor;

        public QaFileFinder()
        {
            _pathFinder = new PathFinder();
            _detector = new StructureDetector();
            _dateExtractor = new FileDateExtractor();
        }

        public List<string> Find(string folder, QaStructure structure)
        {
            var files = _pathFinder
                .Find(folder)
                .Where(x=> _detector.Match(x, structure))
                .ToList();

            if (structure.CompareFilesMethod == CompareFilesMethod.FourMonths)
            {
                files = getFourFiles(files);
            }
            
            return files;
        }

        private List<string> getFourFiles(List<string> list)
        {
            //filter out wrong files
            list = list.Select(x =>
            {
                if (_dateExtractor.ExtractMonth(x) == null)
                {
                    Lo.Wl("File '" + x + "' doesn't have date in file name");
                    return null;
                }
                return x;
            }).Where(x => x != null).ToList();

            var maxDate = list.Max(x => _dateExtractor.ExtractMonth(x));
            if (maxDate == null)
            {
                return list;
            }

            var currentMonth = list.First(x => _dateExtractor.ExtractMonth(x) == maxDate.Value);
            var previousMonth = list.FirstOrDefault(x => _dateExtractor.ExtractMonth(x) == maxDate.Value.AddMonths(-1));
            var previousYear = list.FirstOrDefault(x => _dateExtractor.ExtractMonth(x) == maxDate.Value.AddYears(-1));
            var lastDecember = list.FirstOrDefault(x => _dateExtractor.ExtractMonth(x) == new DateTime(maxDate.Value.Year - 1, 12, 1));

            return new[] {currentMonth, previousMonth, lastDecember, previousYear}.Where(x => x != null).ToList();
        }
    }
}
