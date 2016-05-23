using Qa.Core.Parsers;
using Qa.Core.Structure;

namespace Qa.Core
{
    public static class StructureExtension
    {
        public static CsvParser GetLineParser(this IStructure structure)
        {
            return new CsvParser(structure.Delimiter, structure.TextQualifier);
        }
    }
}
