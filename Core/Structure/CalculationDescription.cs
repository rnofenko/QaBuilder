namespace Qa.Core.Structure
{
    public class CalculationDescription
    {
        public string GroupBy { get; set; }

        public int GroupByIndex { get; set; }

        public CalculationType Type { get; set; }

        public bool IsGrouped()
        {
            return GroupByIndex > 0;
        }
    }
}