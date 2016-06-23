using Qa.Core.Compare;

namespace Qa.Core.Excel
{
    public class FieldPrinterFactory
    {
        public IGroupedFieldPrinter CreateGroupedPrinter(CompareFilesMethod compMethod)
        {
            if (compMethod == CompareFilesMethod.FourMonths)
            {
                return new FourMonthsGroupedFieldPrinter();
            }
            return new DefaultGroupedFieldPrinter();
        }

        public INumberFieldPrinter CreateNumberPrinter(CompareFilesMethod compMethod)
        {
            if (compMethod == CompareFilesMethod.FourMonths)
            {
                return new FourMonthsNumberFieldPrinter();
            }
            return new DefaultNumberFieldPrinter();
        }
    }
}