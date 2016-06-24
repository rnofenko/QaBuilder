using System;

namespace Qa.Core.Excel
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
                var value = x.Value.Double();
                if (value == null)
                {
                    x.Cursor.SetAsDanger(x.Pos);
                }
                
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