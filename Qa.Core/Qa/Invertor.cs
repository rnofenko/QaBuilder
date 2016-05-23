using System.Collections.Generic;
using System.Linq;
using Qa.Core.Parsers;

namespace Qa.Core.Qa
{
    public class Invertor
    {
        public List<ParsedFile> Invert(List<ParsedFile> files)
        {
            foreach (var parsedFile in files)
            {
                invert(parsedFile);
            }
            return files;
        }

        private void invert(ParsedFile parsedFile)
        {
            foreach (var calculatedField in parsedFile.Fields.Where(x => x.Field.Invert))
            {
                calculatedField.Number = -calculatedField.Number;
                calculatedField.GroupedNumbers = calculatedField.GroupedNumbers.ToDictionary(x => x.Key, x => -x.Value);
            }
        }
    }
}
