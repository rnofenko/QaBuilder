using System.Text.RegularExpressions;
using Qa.Core.Combines;
using Qa.Core.Structure;

namespace Qa.File
{
    public class TransactionCombiner
    {
        public void Combine(Settings settings)
        {
            var combiner = new FileCombiner();
            var combineSettings = new CombineSettings(settings) {HeaderRowsCount = 1, FileMask = "CD_Transactions*.csv" };
            combiner.Combine(combineSettings);
        }
    }
}