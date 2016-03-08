using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Transforms
{
    public class ReportTransformer
    {
        public List<RawReportField> Transform(List<RawReportField> sourceFields, TransformStructure structure)
        {
            var targetFields = new List<RawReportField>();
            foreach (var fieldLink in structure.Fields)
            {
                if (fieldLink.Fields.IsEmpty())
                {
                    targetFields.Add(copyOneField(fieldLink, sourceFields));
                }
                else
                {
                    targetFields.Add(combineFields(fieldLink, sourceFields));
                }
            }
            return targetFields;
        }

        private RawReportField combineFields(FieldLink link, List<RawReportField> fields)
        {
            var selected = fields.Where(x => link.Fields.Contains(x.Name)).ToList();
            var rawField = new RawReportField(link.Name, link.Name, selected.First().Type) { Sum = selected.Sum(x => x.Sum) };
            return rawField;
        }

        private RawReportField copyOneField(FieldLink link, List<RawReportField> fields)
        {
            var rawField = fields.First(x => x.Name == link.Name);
            return rawField.Clone();
        }
    }
}