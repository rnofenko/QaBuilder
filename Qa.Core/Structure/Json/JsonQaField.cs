using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Qa.Core.Structure.Json
{
    public class JsonQaField
    {
        public string Title { get; set; }

        public string Field { get; set; }

        public string Code { get; set; }

        public string WeightField { get; set; }

        public BinSettings Bins { get; set; }

        public string FilterExpression { get; set; }

        public string GroupBy { get; set; }

        public NumberFormat NumberFormat { get; set; }

        public string TranslateFunction { get; set; }

        public CalculationType Calculation { get; set; }

        public FieldStyle Style { get; set; }

        public Dictionary<string, string> Translate { get; set; }

        public QaField Convert(List<Field> fields)
        {
            var field = getField(fields);
            var qa = new QaField
            {
                Calculation = Calculation,
                Code = Code,
                Bins = Bins,
                FieldIndex = fields.FindIndex(x => x.Name == Field),
                FilterExpression = FilterExpression,
                NumberFormat = NumberFormat == NumberFormat.None ? field.NumberFormat : NumberFormat,
                Style = Style,
                Title = Title ?? Field,
                Translate = Translate ?? new Dictionary<string, string>(),
                WeightFieldIndex = fields.FindIndex(x => x.Name == WeightField),
                TranslateFunction = TranslateFunction != null ? TranslateFunction.ToUpper() : null
            };
            resolveGroupping(qa, fields);
            
            if (field.Type == DType.Numeric)
            {
                if (qa.Calculation == CalculationType.None)
                {
                    qa.Calculation = CalculationType.Sum;
                }
            }
            if (qa.Calculation == CalculationType.CountUnique)
            {
                qa.NumberFormat = NumberFormat.Integer;
            }
            return qa;
        }

        private void resolveGroupping(QaField qa, List<Field> fields)
        {
            if (GroupBy.IsEmpty())
            {
                return;
            }
            qa.Title = qa.Title ?? GroupBy;

            qa.GroupByIndexes = GroupBy.Split(',')
                .Select(x => x.Trim())
                .Select(x => fields.FindIndex(f => f.Name == x))
                .Where(x => x > -1)
                .ToArray();

            if (qa.GroupByIndexes.Length == 0)
            {
                qa.GroupByIndexes = null;
            }
        }

        private Field getField(List<Field> fields)
        {
            if (Field.IsEmpty())
            {
                if (Calculation == CalculationType.Count)
                {
                    return new Field { Name = "ROWS_COUNT", NumberFormat = NumberFormat.Integer, Type = DType.Numeric };
                }
                if (Calculation == CalculationType.Custom)
                {
                    return new Field();
                }
                throw new WarningException(string.Format("Field attribute must be filled in qa.fields section. Title is {0} wasn't found.", Title));
            }
            
            var field = fields.FirstOrDefault(x => x.Name == Field);
            if (field == null)
            {
                throw new WarningException(string.Format("Field with name {0} wasn't found.", Field));
            }

            return field;
        }
    }
}