using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Q2.Core.Structure;
using Qa.Core.Parsers;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Format
{
    public class Formatter
    {
        private List<Field> _fields;
        private LineParser _lineParser;
        private string _destinationDelimeter;
        
        public void Format(FormattingFile file)
        {
            _destinationDelimeter = file.FormatStructure.DestinationDelimiter;
            var formatStructure = file.FormatStructure;
            _lineParser = formatStructure.GetLineParser();
            _fields = file.FormatStructure.Fields;
            var rowCount = 1;
            using (var reader = new StreamReader(file.SourcePath))
            {
                using (var writer = new StreamWriter(file.DestinationPath))
                {
                    for (var i = 0; i < formatStructure.RowsInHeader; i++)
                    {
                        reader.ReadLine();
                    }
                    writer.WriteLine(string.Join(_destinationDelimeter, _fields.Select(x => x.Name)));

                    var line = reader.ReadLine();
                    writer.WriteLine(processLine(line));

                    while ((line = reader.ReadLine())!=null)
                    {
                        writer.WriteLine(processLine(line));
                        rowCount++;
                    }
                }
            }
            Lo.Wl().Wl(string.Format("Processed {0} rows.", rowCount));
        }

        private string processLine(string line)
        {
            var parts = new string[_fields.Count];
            var matches = _lineParser.Parse(line);

            for (var i = 0; i < _fields.Count; i++)
            {
                var value = matches[i];
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
                        value = string.Format("{0:0.00}", parsed);
                    }
                    else if (field.NumberFormat == NumberFormat.Rate)
                    {
                        value = string.Format("{0:0.0000}", parsed);
                    }
                    else
                    {
                        value = string.Format("{0:0}", parsed);
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
            }
            return string.Join(_destinationDelimeter, parts);
        }
    }
}