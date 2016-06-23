namespace Qa.Core.Compare
{
    public class ValueComparerFactory
    {
        public IValueComparer Create(CompareFilesMethod method)
        {
            if (method == CompareFilesMethod.FourMonths)
            {
                return new FourMonthsValueComparer();
            }
            return new OneByOneValueComparer();
        }
    }
}