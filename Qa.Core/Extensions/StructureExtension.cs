using Q2.Core.Structure;
using Qa.Core.Parsers;
using Qa.Core.Structure;

namespace Qa.Core
{
    public static class StructureExtension
    {
        public static LineParser GetLineParser(this IStructure structure)
        {
            return new LineParser(structure.Delimiter, structure.TextQualifier);
        }
    }
}
