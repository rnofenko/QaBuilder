﻿using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Parsers
{
    public class ParsedFile
    {
        public string SplitValue { get; set; }

        public string Path { get; set; }

        public List<CalculatedField> Fields { get; set; }
    }
}