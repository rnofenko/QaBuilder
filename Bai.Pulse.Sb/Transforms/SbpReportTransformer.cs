using System.Collections.Generic;
using System.Linq;
using Qa.Bai.Pulse.Sb.Collectors;
using Qa.Bai.Sbp;
using Qa.Core.Structure;
using Qa.Core.Transforms;

namespace Qa.Bai.Pulse.Sb.Transforms
{
    public class SbpReportTransformer
    {
        private readonly ReportTransformer _transformer;

        public SbpReportTransformer()
        {
            _transformer = new ReportTransformer();
        }

        public void Transform(List<RawReport> rawReports)
        {
            foreach (var rawReport in rawReports)
            {
                foreach (var transformStructure in rawReport.Structure.Transformations)
                {
                    var transformedReport = transform(rawReport, transformStructure);
                    rawReport.TransformedReports.Add(transformStructure.Name, transformedReport);
                }
            }
        }

        private RawReport transform(RawReport rawReport, TransformStructure structure)
        {
            var subReports = new Dictionary<string, RawSubReport>();
            foreach (var subReport in rawReport.SubReports.Where(x => x.Key != QaSettings.National))
            {
                var transformedFields = _transformer.Transform(subReport.Value.Fields, structure);
                subReports.Add(subReport.Key, new RawSubReport {RowsCount = subReport.Value.RowsCount, Fields = transformedFields });
            }
            
            var result = new RawReport(subReports.Values.First().Fields.Select(x => x.Description)) {SubReports = subReports, Path = rawReport.Path };
            return result;
        }
    }
}
