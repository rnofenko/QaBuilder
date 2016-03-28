using System.Collections.Generic;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class UniqueValuesField : BaseField
    {
        private readonly Dictionary<string, string> _translate;

        public UniqueValuesField(FieldPack pack)
            : base(pack.Description)
        {
            _translate = pack.Description.Translate ?? new Dictionary<string, string>();
            Keys = pack.UniqueValues.Keys;
            ValueLists = pack.UniqueValues.Lists;
        }

        public List<GroupedValuesList> ValueLists { get; set; }

        public List<string> Keys { get; set; }

        public string GetTranslate(string key)
        {
            if (key.IsEmpty())
            {
                return "No Values";
            }

            if (_translate.ContainsKey(key))
            {
                return _translate[key];
            }
            
            return key;
        }

        public static bool IsConvertable(FieldPack pack)
        {
            return pack.Description.SelectUniqueValues;
        }

        public TypedValue GetCurrent(CompareReport file, string key)
        {
            return ValueLists[file.Index].GetCurrent(key);
        }

        public TypedValue GetChange(CompareReport file, string key)
        {
            return ValueLists[file.Index].GetChange(key);
        }
    }
}