using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Compare;
using Qa.Core.Translates;

namespace Qa.Core.Qa
{
    public class Sorter
    {
        public ComparePacket Sort(ComparePacket packet)
        {
            foreach (var groupedField in packet.GroupedFields)
            {
                sort(groupedField);
            }

            return packet;
        }

        private void sort(GroupedField field)
        {
            field.Keys = field.FileValues
                .Last().Values
                .OrderByDescending(x => Math.Abs(x.Count.Current))
                .Select(x => x.Key)
                .ToList();

            careAboutBinsWhichShouldBeTheLast(field);
            careAboutTranslates(field);

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

        private void careAboutBinsWhichShouldBeTheLast(GroupedField field)
        {
            var bins = field.Qa.Bins;
            if (bins == null)
            {
                return;
            }
            var ranges = bins.Ranges.Where(x => x.Last).ToList();
            if (ranges.IsEmpty())
            {
                return;
            }
            foreach (var binName in ranges.Select(x => x.Name).OrderBy(x => x))
            {
                moveToTheEnd(field.Keys, binName);
            }
        }

        private void careAboutTranslates(GroupedField field)
        {
            if (field.Qa.TranslateFunction.IsEmpty() && field.Qa.Translate.IsEmpty())
            {
                return;
            }
            
            moveToTheEnd(field.Keys, Translator.NO_VALUES);
            moveToTheEnd(field.Keys, Translator.OTHER);
        }

        private void moveToTheEnd(List<string> keys, string value)
        {
            if (!keys.Contains(value))
            {
                return;
            }
            keys.Remove(value);
            keys.Add(value);
        }
    }
}
