using System;
using System.IO;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.FileReaders
{
    public class TextFileReader: IFileReader
    {
        private readonly StreamReader _stream;
        private LineParser _parser;

        public TextFileReader(string path, QaStructure structure)
        {
            _stream = new StreamReader(path);
            _parser = structure.GetLineParser();
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }

        public void Skip(int lines)
        {
            for (var i = 0; i < lines; i++)
            {
                _stream.ReadLine();
            }
        }

        public string[] ReadRow()
        {
            var line = _stream.ReadLine();
            if (line == null)
            {
                return null;
            }
            return _parser.Parse(line);
        }
    }

    public class ExcelFileReader: IFileReader
    {
        public ExcelFileReader(string path, QaStructure structure)
        {
            throw new global::System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Skip(int lines)
        {
            throw new NotImplementedException();
        }

        public string[] ReadRow()
        {
            throw new NotImplementedException();
        }
    }

    public interface IFileReader : IDisposable
    {
        void Skip(int lines);

        string[] ReadRow();
    }

    public class FileReaderFactory
    {
        public static IFileReader Create(string path, QaStructure structure)
        {
            if (path.EndsWith(".xls"))
            {
                return new ExcelFileReader(path, structure);
            }
            return new TextFileReader(path, structure);
        }
    }
}
