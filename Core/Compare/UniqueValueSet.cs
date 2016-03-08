using System.Collections.Generic;

namespace Qa.Core.Compare
{
    public class UniqueValueSet
    {
        public List<UniqueValueList> Lists { get; set; }

        public List<string> Keys { get; set; }

        public UniqueValueSet()
        {
            Lists = new List<UniqueValueList>();
        }

        public void Add(UniqueValueList row)
        {
            Lists.Add(row);
        }
    }
}