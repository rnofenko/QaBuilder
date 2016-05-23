using System.IO;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.FileReaders
{
    public class TextFileReader: IFileReader
    {
        private StreamReader _stream;
        private readonly CsvParser _parser;
        private readonly string _path;

        public TextFileReader(string path, IStructure structure)
        {
            _path = path;
            _parser = structure.GetLineParser();
        }
        
        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        public void Skip(int lines)
        {
            for (var i = 0; i < lines; i++)
            {
                getReader().ReadLine();
            }
        }

        public string[] ReadRow()
        {
            var line = getReader().ReadLine();
            if (line == null)
            {
                return null;
            }
            return _parser.Parse(line);
        }

        public int GetFieldsCount()
        {
            var row = ReadRow();
            return row.Length;
        }

        private StreamReader getReader()
        {
            if (_stream == null)
            {
                _stream = new StreamReader(_path);
            }
            return _stream;
        }
    }
}
