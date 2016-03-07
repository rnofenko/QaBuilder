using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;
using Qa.Core.Transforms;
using Qa.Sbpm.Collectors;

namespace Qa.Sbpm.Transforms
{
    public class SbpmReportTransformer
    {
        private readonly ReportTransformer _transformer;

        public SbpmReportTransformer()
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
                subReports.Add(subReport.Key, new RawSubReport(transformedFields) {RowsCount = subReport.Value.RowsCount});
            }
            
            var result = new RawReport(subReports.Values.First().Fields.Select(x => x.Description)) {SubReports = subReports, Path = rawReport.Path };
            return result;
        }
    }
}
