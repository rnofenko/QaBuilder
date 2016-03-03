using Qa.Core.Transforms;

namespace Qa.BAI_DPB.Transforms
{
    public class BaiDpbReportTransformer
    {
        private readonly ReportTransformer _transformer;

        public BaiDpbReportTransformer()
        {
            _transformer = new ReportTransformer();
        }
    }
}
