using System.IO;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.FileReaders
{
    public class TextFileReader: IFileReader
    {
        private StreamReader _stream;
        private readonly CsvParser _parser;
        private readonly string _path;
        private string _lastLine;

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
            var reader = getReader();
            for (var i = 0; i < lines; i++)
            {
                reader.ReadLine();
            }
        }

        public string[] ParseNextRow()
        {
            _lastLine = getReader().ReadLine();
            if (_lastLine == null)
            {
                return null;
            }
            return _parser.Parse(_lastLine);
        }

        public string ReadNextRow()
        {
            _lastLine = getReader().ReadLine();
            return _lastLine;
        }

        public string GetLastLine()
        {
            return _lastLine;
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
