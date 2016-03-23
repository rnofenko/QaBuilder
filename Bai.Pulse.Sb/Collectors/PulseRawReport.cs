﻿using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Bai.Pulse.Sb.Collectors
{
    public class PulseRawReport
    {
        public string Path { get; set; }

        public FileStructure Structure { get; set; }

        public int FieldsCount { get; set; }

        public List<FieldDescription> Fields { get; set; }

        public Dictionary<string, PulseRawReport> TransformedReports { get; set; }
        
        public Dictionary<string, RawSubReport> SubReports { get; set; }

        public PulseRawReport(IEnumerable<FieldDescription> fields)
        {
            Fields = fields.ToList();
            SubReports = new Dictionary<string, RawSubReport>();
            TransformedReports = new Dictionary<string, PulseRawReport>();
        }

        public RawSubReport GetSubReport(string state)
        {
            if (!SubReports.ContainsKey(state))
            {
                SubReports.Add(state, new RawSubReport {Fields = Fields.Select(x => new RawReportField(x, 0)).ToList()});
            }
            return SubReports[state];
        }
    }
}