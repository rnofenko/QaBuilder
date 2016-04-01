﻿namespace Qa.Core.Structure
{
    public interface IStructure
    {
        string Delimiter { get; }

        string TextQualifier { get; }

        int FieldsCount { get; }
    }
}