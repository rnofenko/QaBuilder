﻿using System;

namespace Qa.Core.Calculations
{
    public static class Calculator
    {
        public static double? ChangeInPercent(double current, double? previous)
        {
            if (previous == null)
            {
                return null;
            }

            if (Math.Abs(previous.Value) < 0.0001d && Math.Abs(current) < 0.0001d)
            {
                return 0;
            }

            if (Math.Abs(previous.Value) < 0.0001d)
            {
                return null;
            }
            if (Math.Abs(current) < 0.0001d)
            {
                return -1;
            }
            return current * 1.0/ previous - 1;
        }

        public static double Portion(double value, double total)
        {
            if (Math.Abs(value) < 0.0001d || Math.Abs(total) < 0.0001d)
            {
                return 0;
            }
            return value * 1.0 / total * 100;
        }

        public static double? AbsoluteChange(double current, double? previous)
        {
            if (previous == null)
            {
                return null;
            }
            return Math.Abs(current - previous.Value);
        }
    }
}