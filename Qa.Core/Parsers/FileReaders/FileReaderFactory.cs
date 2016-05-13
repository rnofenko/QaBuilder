using Qa.Core.Structure;

namespace Qa.Core.Parsers.FileReaders
{
    public class FileReaderFactory
    {
        public static IFileReader Create(string path, IStructure structure)
        {
            if (path.EndsWith(".xls"))
            {
                return new ExcelFileReader(path);
            }
            return new TextFileReader(path, structure);
        }
    }
}