using System.Collections.Generic;
using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Parsers
{
    public class ParsedBatch
    {
        public List<ParsedFile> Files { get; set; }

        public string Path { get; set; }

        public void Sort()
        {
            Files = Files.OrderBy(x => x.SplitValue).ToList();
        }

        public void CreateIfAbsent(string splitValue, QaStructure structure)
        {
            if (Files.Any(x => x.SplitValue == splitValue))
            {
                return;
            }
            Files.Add(new ParsedFile
            {
                SplitValue = splitValue,
                Path = Path,
                Fields = structure.Fields.Select(x => new CalculatedField(x, 0)).ToList()
            });
            Sort();
        }
    }
}