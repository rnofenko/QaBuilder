using System.Collections.Generic;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class GroupedField : BaseField
    {
        private readonly Dictionary<string, string> _translate;

        public GroupedField(FieldPack pack)
            :base(pack.Description)
        {
            _translate = pack.Description.Translate ?? new Dictionary<string, string>();
            Keys = pack.GroupedNumbers.Keys;
            ValueLists = pack.GroupedNumbers.Lists;
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