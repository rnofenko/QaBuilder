using System.Collections.Generic;

namespace Qa.Core.Compare
{
    public class UniqueValuesComparer
    {
        public List<UniqueValue> Compare(Dictionary<string, int> current, Dictionary<string, int> previous)
        {
            var compared = new List<UniqueValue>();
            foreach (var pair in current)
            {
                int? prevCount = null;
                if (previous != null && previous.ContainsKey(pair.Key))
                {
                    prevCount = previous[pair.Key];
                }

                compared.Add(new UniqueValue { Value = pair.Key, Count = new CompareNumber(pair.Value, prevCount) });
            }
            return compared;
        }
    }
}
