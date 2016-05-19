using System.Collections.Generic;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class GroupedField
    {
        private const string NO_VALUES = "No Values";
        private readonly Dictionary<string, string> _translate;

        public GroupedField(FieldPack pack)
        {
            _translate = pack.Field.Translate ?? new Dictionary<string, string>();
            Keys = pack.GroupedNumbers.Keys;
            FileValues = pack.GroupedNumbers.Lists;
            Title = pack.Field.Title;
            Style = pack.Field.Style;
        }

        public string Title { get; private set; }

        public List<GroupedValuesList> FileValues { get; set; }

        public List<string> Keys { get; set; }

        public FieldStyle Style { get; private set; }

        public string GetTranslate(string key)
        {
            if (key.IsEmpty())
            {
                return NO_VALUES;
            }

            if (_translate.ContainsKey(key))
            {
                return _translate[key];
            }

            return key;
        }

        public TypedValue GetCurrent(FileInformation file, string key)
        {
            return FileValues[file.Index].GetCurrent(key);
        }

        public TypedValue GetChange(FileInformation file, string key)
        {
            return FileValues[file.Index].GetChange(key);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}