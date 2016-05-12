using System.Collections.Generic;
using Q2.Core.Compare;
using Q2.Core.Excel;
using Q2.Core.Extensions;
using Q2.Core.Structure;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class GroupedField
    {
        private readonly Dictionary<string, string> _translate;

        public GroupedField(FieldPack pack)
        {
            _translate = pack.Field.Translate ?? new Dictionary<string, string>();
            Keys = pack.GroupedNumbers.Keys;
            ValueLists = pack.GroupedNumbers.Lists;
            Title = pack.Field.Title;
            Style = pack.Field.Style;
        }

        public string Title { get; private set; }

        public List<GroupedValuesList> ValueLists { get; set; }

        public List<string> Keys { get; set; }

        public FieldStyle Style { get; private set; }

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

        public TypedValue GetCurrent(FileInformation file, string key)
        {
            return ValueLists[file.Index].GetCurrent(key);
        }

        public TypedValue GetChange(FileInformation file, string key)
        {
            return ValueLists[file.Index].GetChange(key);
        }
    }
}