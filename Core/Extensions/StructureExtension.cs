using Qa.Core.Collectors;
using Qa.Core.Structure;

namespace Qa.Core
{
    public static class StructureExtension
    {
        public static LineParser GetLineParser(this FileStructure structure)
        {
            return new LineParser(structure.Delimiter, structure.TextQualifier);
        }

        public static LineParser GetLineParser(this FormatStructure structure)
        {
            return new LineParser(structure.Delimiter, structure.TextQualifier);
        }
    }
}
