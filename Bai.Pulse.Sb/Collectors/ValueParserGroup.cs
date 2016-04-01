using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Qa.Core.Collectors;
using Qa.Core.Structure;

namespace Qa.Bai.Pulse.Sb.Collectors
{
    public class ValueParserGroup : IDisposable
    {
        public ValueParserGroup(FileStructure structure)
        {
            _structure = structure;
            _subReportIndex = _structure.Fields.FindIndex(f => f.Name == QaSettings.Field.State);
            _parsers = new Dictionary<string, ValueParser>();
        }

        private readonly FileStructure _structure;
        private readonly int _subReportIndex;
        private readonly Dictionary<string, ValueParser> _parsers;

        public int RowsCount { get; private set; }

        public void Parse(string line)
        {
            string[] parts;
            //if (_structure.TextQualifier == "")
            {
                parts = line.Split(new[] { _structure.Delimiter }, StringSplitOptions.None);
            }
            /*else
            {
                string pattern = $"{_structure.TextQualifier}\\s*{_structure.Delimiter}\\s*{_structure.TextQualifier}";
                parts = Regex.Split(line.Substring(1, line.Length - 2), pattern);
            }*/
            var parser = getParser(parts[_subReportIndex]);
            parser.Parse(parts);
            RowsCount++;
        }

        private ValueParser getParser(string state)
        {
            if (!_parsers.ContainsKey(state))
            {
                _parsers.Add(state, new ValueParser(_structure.Fields));
            }
            return _parsers[state];
        }

        public void Dispose()
        {
            foreach (var valueParser in _parsers)
            {
                valueParser.Value.Dispose();
            }
            _parsers.Clear();
        }

        public Dictionary<string, RawSubReport> GetSubReports()
        {
            var reports = new Dictionary<string, RawSubReport>();
            foreach (var pair in _parsers)
            {
                reports.Add(pair.Key, new RawSubReport
                {
                    Fields = pair.Value.GetResultFields(),
                    RowsCount = pair.Value.RowsCount
                });
            }
            return reports;
        }
    }
}