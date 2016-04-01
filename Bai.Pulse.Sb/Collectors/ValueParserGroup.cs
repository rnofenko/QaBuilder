using System;
using System.Collections.Generic;
using Qa.Core;
using Qa.Core.Collectors;
using Qa.Core.Structure;

namespace Qa.Bai.Pulse.Sb.Collectors
{
    public class ValueParserGroup : IDisposable
    {
        private readonly FileStructure _structure;
        private readonly int _subReportIndex;
        private readonly Dictionary<string, ValueParser> _parsers;
        private readonly LineParser _lineParser;

        public int RowsCount { get; private set; }

        public ValueParserGroup(FileStructure structure)
        {
            _structure = structure;
            _subReportIndex = _structure.Fields.FindIndex(f => f.Name == QaSettings.Field.State);
            _parsers = new Dictionary<string, ValueParser>();
            _lineParser = structure.GetLineParser();
        }
        
        public void Parse(string line)
        {
            var parts = _lineParser.Parse(line);
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