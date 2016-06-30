using System;

namespace Qa.Core.Parsers.FileReaders
{
    public interface IFileReader : IDisposable
    {
        void Skip(int lines);

        string[] ParseNextRow();

        string ReadNextRow();

        string GetLastLine();
    }
}