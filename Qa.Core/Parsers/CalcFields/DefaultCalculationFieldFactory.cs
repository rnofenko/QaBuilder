using System;
using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Parsers.CalcFields
{
    public class DefaultCalculationFieldFactory : ICalculationFieldFactory
    {
        public ICalculationField Get(QaField field, List<Field> allFields)
        {
            if (field.Calculation == CalculationType.Count)
            {
                if (field.Group)
                {
                    return new CalcGroupCountField(field);
                }
                return new CalcCountField(field);
            }
            if (field.Calculation == CalculationType.CountUnique)
            {
                if (field.Group)
                {
                    return new CalcGroupUniqueCountField(field);
                }
                return new CalcUniqueCountField(field);
            }
            if (field.Calculation == CalculationType.Sum)
            {
                if (field.Group)
                {
                    return new CalcGroupedSumField(field, allFields);
                }
                return new CalcSumField(field, allFields);
            }
            if (field.Calculation == CalculationType.Average)
            {
                if (!field.Group)
                {
                    return new CalcAverageField(field);
                }
            }
            if (field.Calculation == CalculationType.WeightedAverage)
            {
                if (!field.Group)
                {
                    return new CalcWeightedAverageField(field);
                }
            }
            throw new NotSupportedException(string.Format("Calculation '{0}' is not supported.", field.Calculation));
        }
    }
}