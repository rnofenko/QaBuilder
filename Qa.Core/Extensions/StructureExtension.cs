using Q2.Core.Collectors;
using Q2.Core.Structure;

namespace Q2.Core.Extensions
{
    public static class StructureExtension
    {
        public static LineParser GetLineParser(this IStructure structure)
        {
            return new LineParser(structure.Delimiter, structure.TextQualifier);
        }
    }
}
