using System.Collections.Generic;
using System.Linq;
using Qa.Core.Compare;
using Qa.Core.System;

namespace Qa.Core.Structure
{
    public class QaFileFinder
    {
        private readonly PathFinder _pathFinder;
        private readonly StructureDetector _detector;
        private readonly FourMonthsFileDetector _fourMonthsFileDetector;

        public QaFileFinder()
        {
            _pathFinder = new PathFinder();
            _detector = new StructureDetector();
            _fourMonthsFileDetector = new FourMonthsFileDetector();
        }

        public List<string> Find(string folder, QaStructure structure)
        {
            var files = _pathFinder
                .Find(folder)
                .Where(x=> _detector.Match(x, structure))
                .ToList();

            if (structure.CompareFilesMethod == CompareFilesMethod.FourMonths)
            {
                files = _fourMonthsFileDetector.FindAllFour(files).Where(x => x != null).ToList();
            }
            
            return files;
        }
    }
}
