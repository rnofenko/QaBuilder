using System.IO;

namespace Qa.Core.Parsers.FileReaders
{
    public class TextFileReader: IFileReader
    {
        private StreamReader _stream;
        private readonly ICsvParser _parser;
        private readonly string _path;
        private string _lastLine;

        public TextFileReader(string path, ICsvParser parser)
        {
            _path = path;
            _parser = parser;
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
            var reader = getReader();
            for (var i = 0; i < lines; i++)
            {
                RowNumber++;
                reader.ReadLine();
            }
        }

        public string[] ParseNextRow()
        {
            _lastLine = getReader().ReadLine();
            RowNumber++;
            if (_lastLine == null)
            {
                return null;
            }
            return _parser.Parse(_lastLine);
        }

        public string ReadNextRow()
        {
            _lastLine = getReader().ReadLine();
            RowNumber++;
            return _lastLine;
        }

        public string GetLastLine()
        {
            return _lastLine;
        }

        public int RowNumber { get; private set; }

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
