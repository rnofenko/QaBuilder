using Qa.Core.Combines;
using Qa.Core.Structure;

namespace Qa.ImportFiles
{
    public class TransactionCombiner
    {
        public void Combine(Settings settings)
        {
            var combiner = new FileCombiner();
            var combineSettings = new CombineSettings(settings) {HeaderRowsCount = 1, FileMask = "CD_Transactions*.txt" };
            combiner.Combine(new CombineSettings(settings) { HeaderRowsCount = 1, FileMask = "CD_Transactions*.txt" });
        }
    }
}