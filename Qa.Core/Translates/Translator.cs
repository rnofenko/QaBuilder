using System;
using System.Collections.Generic;
using System.Linq;
using Qa.Core.Parsers;
using Qa.Core.Structure;

namespace Qa.Core.Translates
{
    public class Translator
    {
        public const string NO_VALUES = "No Values";
        public const string OTHER = "Other";

        private static readonly Lazy<Dictionary<string, ITranslator>> _translators = new Lazy<Dictionary<string, ITranslator>>(() => new Dictionary<string, ITranslator>
        {
            {Usa50StatesTranslator.NAME, new Usa50StatesTranslator()}
        });

        public List<ParsedFile> Translate(List<ParsedFile> files)
        {
            foreach (var file in files)
            {
                foreach (var field in file.Fields.Where(x => x.Field.Group))
                {
                    var translated = new Dictionary<string, double>();
                    foreach (var groupedNumber in field.GroupedNumbers)
                    {
                        var newKey = getTranslate(groupedNumber.Key, field.Field);
                        if (translated.ContainsKey(newKey))
                        {
                            translated[newKey] += groupedNumber.Value;
                        }
                        else
                        {
                            translated.Add(newKey, groupedNumber.Value);
                        }
                    }
                    field.GroupedNumbers = translated;
                }
            }

            return files;
        }

        private string getTranslate(string key, QaField field)
        {
            if (field.TranslateFunction.IsNotEmpty())
            {
                return _translators.Value[field.TranslateFunction].GetTranslate(key, field);
            }

            if (key.IsEmpty())
            {
                return NO_VALUES;
            }
            
            if (field.Translate.ContainsKey(key))
            {
                return field.Translate[key];
            }

            return key;
        }
    }
}
