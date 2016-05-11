using System;

namespace Q2.Core.Excel
{
    public class StyleConditions
    {
        public static Action<StyleConditionArgs> ChangePercent = x =>
        {
            if (x.Value.Value == null)
            {
                x.Cursor.SetAsDanger(x.Pos);
            }
            else
            {
                var value = Math.Abs(x.Value.Double());
                if (value > 0.35)
                {
                    x.Cursor.SetAsDanger(x.Pos);
                }
                else if (value > 0.20)
                {
                    x.Cursor.SetAsWarning(x.Pos);
                }
            }
        };
    }
}