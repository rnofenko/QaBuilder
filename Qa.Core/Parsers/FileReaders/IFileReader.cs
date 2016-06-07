using System;

namespace Qa.Core.Parsers.FileReaders
{
    public interface IFileReader : IDisposable
    {
        void Skip(int lines);

        string[] ReadRow();

        int GetFieldsCount();

        string GetLastLine();
    }
}