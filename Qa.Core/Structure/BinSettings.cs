using System.Collections.Generic;

namespace Qa.Core.Structure
{
    public class BinSettings
    {
        public List<BinRange> Ranges { get; set; }

        public BinMethod Method { get; set; }

        public BinSource Source { get; set; }
    }
}