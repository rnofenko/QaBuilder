using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Parsers.CalcFields;
using Qa.Core.Structure;
using Qa.Core.System;

namespace Qa.Core.Parsers
{
    public class ValueParser : IDisposable
    {
        private readonly List<ICalculationField> _fields;

        public int RowsCount { get; private set; }

        public ValueParser(QaStructure structure)
        {
            var fieldFactory = ServiceLocator.CalculationFieldFactory;
            _fields = structure.Fields.Select(x=>fieldFactory.Get(x, structure.SourceFields)).ToList();
        }

        public List<CalculatedField> GetResultFields()
        {
            var resultFields = _fields.Select(x => new CalculatedField(x)).ToList();
            Dispose();

            return resultFields;
        }

        public void Parse(string[] parts)
        {
            RowsCount++;
            foreach (var field in _fields)
            {
                field.Calc(parts);
            }
        }
        
        public void Dispose()
        {
            _fields.Clear();
        }
    }
}