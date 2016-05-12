namespace Q2.Core.Structure
{
    public interface IStructure
    {
        string Delimiter { get; }

        string TextQualifier { get; }

        int SkipRows { get; }

        int RowsInHeader { get; }

        string FileMask { get; }

        int CountOfFieldsInFile { get; }
    }
}