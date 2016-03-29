using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    for (var i = 0; i < (formatStructure.RowsInHeader ?? formatStructure.Destination.RowsInHeader); i++)
                    {
                        reader.ReadLine();
                    }
                    writer.WriteLine(string.Join(_destinationDelimeter, _fields.Select(x => x.Title)));

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
            var match = Regex.Match(line, _regexPattern);

            for (var i = 0; i < _fields.Count; i++)
            {
                var value = match.Value;
                var field = _fields[i];
                if (field.Type == DType.Numeric)
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
                    else if (field.NumberFormat == NumberFormat.Rate)
                    {
                        value = $"{parsed:0.0000}";
                    }
                    else
                    {
                        value = $"{parsed:0}";
                    }
                }
                else if (field.Type == DType.Date)
                {
                    if (value == "31DEC9999")
                    {
                        value = "";
                    }

                    if (field.DateFormat.IsNotEmpty())
                    {
                        var date = DateTime.Parse(value);
                        value = date.ToString(field.DateFormat);
                    }
                }
                else
                {
                    if (value == ".")
                    {
                        value = "";
                    }
                }

                parts[i] = value;

                match = match.NextMatch();
            }
            return string.Join(_destinationDelimeter, parts);
        }
    }
}