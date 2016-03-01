using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qa.Structure;
using Qa.System;

namespace Qa.Format
{
    public class Formatter
    {
        private readonly FormatSettings _settings;
        private bool _removeQuotes;
        private List<FieldDescription> _fields;

        public Formatter(FormatSettings settings)
        {
            _settings = settings;
        }
        
        public void Format(FormattingFile file)
        {
            _fields = file.Structure.Fields;
            var rowCount = 1;
            using (var reader = new StreamReader(file.SourcePath))
            {
                using (var writer = new StreamWriter(file.DestinationPath))
                {
                    for (var i = 0; i < _settings.SkipRows; i++)
                    {
                        reader.ReadLine();
                    }
                    var line = reader.ReadLine();
                    analyzeLine(line);
                    writer.WriteLine(processLine(line));

                    while ((line = reader.ReadLine())!=null)
                    {
                        writer.WriteLine(processLine(line));
                        rowCount++;
                    }
                }
            }
            Lo.Wl().Wl($"Processed {rowCount} rows.");
        }

        private string processLine(string line)
        {
            var parts = line.Split(new[] { _settings.SourceDelimeter }, StringSplitOptions.None);
            if (_removeQuotes)
            {
                parts = parts.Select(x => x.Substring(1, x.Length - 2)).ToArray();
            }
            for (var i = 0; i < parts.Length; i++)
            {
                if (_fields[i].Type == DType.Float)
                {
                    var unparsed = parts[i];
                    var parsed = 0d;
                    if (unparsed != ".")
                    {
                        parsed = double.Parse(unparsed);
                    }
                    parts[i] = $"{parsed:0.00}";
                }
            }
            return string.Join("|", parts);
        }

        private void analyzeLine(string line)
        {
            var parts = line.Split(new[] { _settings.SourceDelimeter }, StringSplitOptions.None);
            var allHaveQuotes = true;
            foreach (var part in parts)
            {
                if (part.StartsWith("\"") && part.EndsWith("\"")) continue;
                allHaveQuotes = false;
                break;
            }
            _removeQuotes = allHaveQuotes;
        }
    }
}