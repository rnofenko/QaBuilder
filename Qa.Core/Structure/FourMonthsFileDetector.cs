using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Parsers;

namespace Qa.Core.Structure
{
    public class FourMonthsFileDetector
    {
        private readonly FileDateExtractor _dateExtractor;
        
        public FourMonthsFileDetector()
        {
            _dateExtractor = new FileDateExtractor();
        }

        public string FindTheCurrent(List<string> filenames)
        {
            var maxDate = filenames.Max(x => _dateExtractor.ExtractMonth(x));
            if (maxDate == null)
            {
                return null;
            }

            return filenames.First(x => _dateExtractor.ExtractMonth(x) == maxDate.Value);
        }

        public string FindMoM(List<string> filenames)
        {
            var currentDate = _dateExtractor.ExtractMonth(FindTheCurrent(filenames));
            if (currentDate == null)
            {
                return null;
            }

            return filenames.FirstOrDefault(x => _dateExtractor.ExtractMonth(x) == currentDate.Value.AddMonths(-1));
        }

        public string FindYtD(List<string> filenames)
        {
            var currentDate = _dateExtractor.ExtractMonth(FindTheCurrent(filenames));
            if (currentDate == null)
            {
                return null;
            }

            return filenames.FirstOrDefault(x => _dateExtractor.ExtractMonth(x) == currentDate.Value.AddYears(-1));
        }

        public string FindYearAgo(List<string> filenames)
        {
            var currentDate = _dateExtractor.ExtractMonth(FindTheCurrent(filenames));
            if (currentDate == null)
            {
                return null;
            }

            return filenames.FirstOrDefault(x => _dateExtractor.ExtractMonth(x) == new DateTime(currentDate.Value.Year - 1, 12, 1));
        }

        public List<string> FindAllFour(List<string> filenames)
        {
            return new List<string> {FindTheCurrent(filenames), FindMoM(filenames), FindYtD(filenames), FindYearAgo(filenames)};
        }
    }
}