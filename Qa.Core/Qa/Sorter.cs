using System.Collections.Generic;
using System.Linq;
using Qa.Core.Compare;

namespace Qa.Core.Qa
{
    public class Sorter
    {
        public List<ComparePacket> Sort(List<ComparePacket> result)
        {
            foreach (var comparePacket in result)
            {
                sort(comparePacket);
            }

            return result;
        }

        private void sort(ComparePacket packet)
        {
            foreach (var groupedField in packet.GroupedFields)
            {
                sort(groupedField);
            }
        }

        private void sort(GroupedField field)
        {
            field.Keys = field.FileValues
                .Last().Values
                .OrderByDescending(x => x.Count.Current)
                .Select(x => x.Key)
                .ToList();

            for (var i = 0; i < field.Keys.Count; i++)
            {
                var key = field.Keys[i];
                foreach (var fileValues in field.FileValues)
                {
                    var currentPair = fileValues.Values.FirstOrDefault(x => x.Key == key) ?? new KeyNumberPair {Key = key, Count = new CompareNumber(0, 0)};
                    fileValues.Values.Remove(currentPair);
                    fileValues.Values.Insert(i, currentPair);
                }
            }
        }
    }
}
