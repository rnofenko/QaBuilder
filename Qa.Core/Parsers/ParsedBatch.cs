using System.Collections.Generic;
using System.Linq;
using Q2.Core.Structure;
using Qa.Core.Parsers;
using Qa.Core.Structure;

namespace Q2.Core.Parsers
{
    public class ParsedBatch
    {
        public List<ParsedFile> Files { get; set; }

        public QaStructure Structure { get; set; }

        public string Path { get; set; }

        public void Sort()
        {
            Files = Files.OrderBy(x => x.SplitValue).ToList();
        }

        public void CreateIfAbsent(string splitValue)
        {
            if (Files.Any(x => x.SplitValue == splitValue))
            {
                return;
            }
            Files.Add(new ParsedFile
            {
                SplitValue = splitValue,
                Structure = Structure,
                Path = Path,
                Fields = Structure.Fields.Select(x => new CalculatedField(x, 0)).ToList()
            });
            Sort();
        }
    }
}