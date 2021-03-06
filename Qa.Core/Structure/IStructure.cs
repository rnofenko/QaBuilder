﻿namespace Qa.Core.Structure
{
    public interface IStructure
    {
        string Name { get; }

        string Delimiter { get; }

        string TextQualifier { get; }

        int SkipRows { get; }

        int RowsInHeader { get; }

        string FileMask { get; }

        int CountOfSourceFields { get; }
    }
}