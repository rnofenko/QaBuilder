using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Q2.Core.Extensions;
using Q2.Core.Structure;

namespace Qa.Core.Structure.Json
{
    public class JsonQaField
    {
        public string Title { get; set; }

        public string Field { get; set; }

        public string Code { get; set; }

        public string WeightField { get; set; }

        public string FilterExpression { get; set; }

        public bool Group { get; set; }

        public string GroupBy { get; set; }

        public NumberFormat NumberFormat { get; set; }

        public CalculationType Calculation { get; set; }

        public Dictionary<string, string> Translate { get; set; }

        public QaField Convert(List<Field> fields)
        {
            var field = getField(fields);
            var qa = new QaField
            {
                Calculation = Calculation,
                Code = Code,
                FieldIndex = fields.FindIndex(x => x.Name == Field),
                FilterExpression = FilterExpression,
                Group = Group,
                NumberFormat = NumberFormat == NumberFormat.None ? field.NumberFormat : NumberFormat,
                Title = Title ?? Field,
                Translate = Translate,
                GroupByIndex = fields.FindIndex(x => x.Name == GroupBy),
                WeightFieldIndex = fields.FindIndex(x => x.Name == WeightField)
            };
            if (qa.GroupByIndex >= 0)
            {
                qa.Group = true;
            }
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