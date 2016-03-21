using System;
using System.IO;
using Qa.Core;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.ImportFiles
{
    public class DateAdder
    {
        private readonly FileFinder _finder;

        public DateAdder()
        {
            _finder = new FileFinder();
        }

        public void Add(Settings settings)
        {
            var filePaths = _finder.Find(settings.WorkingFolder, "CD_Interest*.csv");
            foreach (var filePath in filePaths)
            {
                var name = Path.GetFileName(filePath);
                var monthStr = name.Substring(12, 6);
                var date = new DateTime(int.Parse(monthStr.Substring(0,4)), int.Parse(monthStr.Substring(4, 2)), 1);
                date = date.AddMonths(1).AddDays(-1);

                var tempFileName = filePath + "1";
                using (var outputStream = File.AppendText(tempFileName))
                {
                    using (var stream = new StreamReader(filePath))
                    {
                        for (var i = 0; i < 1; i++)
                        {
                            outputStream.WriteLine(stream.ReadLine() + ",Date");
                        }
                        
                        while (true)
                        {
                            var line = stream.ReadLine();
                            if (line.IsEmpty())
                            {
                                break;
                            }
                            line += "," + date.ToString("MM/dd/yyyy");
                            outputStream.WriteLine(line);
                        }
                    }

                    outputStream.Flush();
                }
                File.Delete(filePath);
                File.Move(tempFileName, filePath);
            }
        }
    }
}