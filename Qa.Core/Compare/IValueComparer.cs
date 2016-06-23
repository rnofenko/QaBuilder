using System.Collections.Generic;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public interface IValueComparer
    {
        GroupedValuesSet Compare(IEnumerable<Dictionary<string, double>> values, QaField field);

        List<CompareNumber> Compare(IEnumerable<double> values);
    }
}