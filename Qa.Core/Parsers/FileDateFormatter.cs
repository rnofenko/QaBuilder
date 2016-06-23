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
            var date = _extractor.Extract(fileName);
            if (date == null)
            {
                return fileName;
            }

            return string.Format("{0:MMMM d, yyyy}", date);
        }
    }
}