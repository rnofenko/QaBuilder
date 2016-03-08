using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class FieldPack
    {
        private readonly FieldDescription _description;

        public DType Type => _description.Type;

        public string Name => _description.Name;

        public string Title => _description.Title ?? _description.Name;

        public bool SelectUniqueValues => _description.SelectUniqueValues;

        public bool CountUniqueValues => _description.CountUniqueValues;

        public UniqueValueSet UniqueValueSet { get; set; }

        public List<CompareNumber> Numbers { get; set; }

        public bool IsNumber => Type == DType.Double || Type == DType.Int || Type == DType.Money || Type == DType.Percent;
        
        public FieldPack(FieldDescription filDescription)
        {
            _description = filDescription;
            Numbers =  new List<CompareNumber>();
        }

        public CompareField GetNumberField(int periodId)
        {
            return new CompareField(_description, Numbers[periodId]);
        }
    }
}