using System.Collections.Generic;
using System.IO;
using Qa.Core.System;
using Qa.System;

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
            combineFiles(filePaths, outputPath);
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
    }
}
