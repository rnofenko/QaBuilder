using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Format
{
    public class Formatter
    {
        private List<FieldDescription> _fields;
        private string _regexPattern;
        private string _destinationDelimeter;

        public void Format(FormattingFile file)
        {
            _destinationDelimeter = file.FormatStructure.Destination.Delimeter;
            var formatStructure = file.FormatStructure;
            var delimeter = formatStructure.Delimeter;
            _fields = file.FormatStructure.Destination.Fields;
            _regexPattern = "((?<=\")[^\"]*(?=\"(" + delimeter + "|$)+)|(?<=" + delimeter + "|^)[^" + delimeter + "\"]*(?=" + delimeter + "|$))";
            var rowCount = 1;
            using (var reader = new StreamReader(file.SourcePath))
            {
                using (var writer = new StreamWriter(file.DestinationPath))
                {
                    for (var i = 0; i < formatStructure.RowsInHeader; i++)
                    {
                        writer.WriteLine(reader.ReadLine());
                    }
                    var line = reader.ReadLine();
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
            var parts = new string[_fields.Count];
            var result = Regex.Split(line, _regexPattern);
            
            for (var i = 0; i < _fields.Count; i++)
            {
                var value = result[i*2 + 1];
                var field = _fields[i];
                if (field.Type == DType.Number)
                {
                    if (value == ".")
                    {
                        value = "0";
                    }
                    var parsed = double.Parse(value);
                    if (field.NumberFormat == NumberFormat.Double || field.NumberFormat == NumberFormat.Money)
                    {
                        value = $"{parsed:0.00}";
                    }
                    else
                    {
                        value = $"{parsed:0}";
                    }
                }
                else if (field.Type == DType.Date)
                {
                    if (field.DateFormat.IsNotEmpty())
                    {
                        var date = DateTime.Parse(value);
                        value = date.ToString(field.DateFormat);
                    }
                }

                parts[i] = value;
            }
            return string.Join(_destinationDelimeter, parts);
        }
    }
}