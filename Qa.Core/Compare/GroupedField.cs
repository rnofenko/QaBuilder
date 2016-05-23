using System.Collections.Generic;
using Qa.Core.Excel;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class GroupedField
    {
        public GroupedField(FieldPack pack)
        {
            Qa = pack.Field;
            Keys = pack.GroupedNumbers.Keys;
            FileValues = pack.GroupedNumbers.Lists;
        }

        public string Title
        {
            get { return Qa.Title; }
        }

        public List<GroupedValuesList> FileValues { get; set; }

        public List<string> Keys { get; set; }

        public QaField Qa { get; private set; }

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