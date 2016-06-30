namespace Qa.Core.Parsers.FileReaders
{
    public class FileReaderFactory
    {
        public static IFileReader Create(string path, ICsvParser parser)
        {
            if (path.EndsWith(".xls"))
            {
                return new ExcelFileReader(path);
            }
            return new TextFileReader(path, parser);
        }
    }
}