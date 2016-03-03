﻿using System;
using Qa.Core.Excel;

namespace Qa.BaiDpb.Excel
{
    public class StyleConditions
    {
        public static Action<StyleConditionArgs> ChangePercent = x =>
        {
            if (Math.Abs(x.Amount) > 0.35)
            {
                x.Cursor.SetAsDanger(x.Pos);
            }
            else if (Math.Abs(x.Amount) > 0.20)
            {
                x.Cursor.SetAsWarning(x.Pos);
            }
        };
    }
}
