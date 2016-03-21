using System.Collections.Generic;
using System.IO;
using Qa.Core.System;

namespace Qa.Core.Combines
{
    public class FileCombiner
    {
        private readonly FileFinder _finder;

        public FileCombiner()
        {
            _finder = new FileFinder();
        }

        public void Combine(CombineSettings settings)
        {
            var filePaths = _finder.Find(settings.WorkingFolder, settings.FileMask);
            var outputPath = Path.Combine(settings.WorkingFolder, "combined.txt");
            if (settings.HeaderRowsCount > 0)
            {
                combineFiles(filePaths, outputPath, settings.HeaderRowsCount);
            }
            else
            {
                combineFiles(filePaths, outputPath);
            }
            Lo.NewPage($"{filePaths.Count} files were combined into {outputPath}.")
              .WaitAnyKey();
        }

        private void combineFiles(IEnumerable<string> filePaths, string outputPath)
        {
            using (var outputStream = File.Create(outputPath))
            {
                foreach (var inputFilePath in filePaths)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        inputStream.CopyTo(outputStream);
                    }
                }
            }
        }

        private void combineFiles(IEnumerable<string> filePaths, string outputPath, int headersRowCount)
        {
            var isFirst = true;

            using (var outputStream = File.AppendText(outputPath))
            {
                foreach (var inputFilePath in filePaths)
                {
                    using (var stream = new StreamReader(inputFilePath))
                    {
                        if (isFirst)
                        {
                            for (var i = 0; i < headersRowCount; i++)
                            {
                                outputStream.WriteLine(stream.ReadLine());
                            }
                            isFirst = false;
                        }
                        else
                        {
                            for (var i = 0; i < headersRowCount; i++)
                            {
                                stream.ReadLine();
                            }
                        }
                        var text = stream.ReadToEnd();
                        outputStream.Write(text);
                    }
                }

                outputStream.Flush();
            }
        }
    }
}
