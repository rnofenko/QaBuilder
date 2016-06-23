namespace Qa.Core.Parsers
{
    public class FileDateFormatter
    {
        private readonly FileDateExtractor _extractor;

        public FileDateFormatter()
        {
            _extractor = new FileDateExtractor();
        }

        public string Format(string fileName)
        {
            var date = _extractor.ExtractDate(fileName);
            if (date != null)
            {
                return string.Format("{0:MMMM d, yyyy}", date);
            }
            date = _extractor.ExtractMonth(fileName);
            if (date != null)
            {
                return string.Format("{0:MMMM, yyyy}", date);
            }

            return fileName;
        }
    }
}