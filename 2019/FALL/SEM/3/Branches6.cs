using System;
using System.Collections.Generic;
using System.Text;

namespace SeminarTasks
{
    class Branches6
    {
        public static (double, double) Find4thVertex(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            double x4, y4;
            var l12 = LineLength(x1, y1, x2, y2);
            var l23 = LineLength(x2, y2, x3, y3);
            var l13 = LineLength(x1, y1, x3, y3);
            double side, side1;
            var diagonal = Max(l12, l23, l13);
            double v1X, v1Y, v2X, v2Y,v3X,v3Y;
            if (diagonal == l12)
            {
                side = l23;
                side1 = l13;
                v1X = x1;
                v1Y = y1;
                v2X = x2;
                v2Y = y2;
                v3X = x3;
                v3Y = y3;
            }
            else
            if (diagonal == l23)
            {
                side = l12;
                side1 = l13;
                v1X = x3;
                v1Y = y3;
                v2X = x2;
                v2Y = y2;
                v3X = x1;
                v3Y = y1;
            }
            else
            {
                side = l12;
                side1 = l23;
                v1X = x1;
                v1Y = y1;
                v2X = x3;
                v2Y = y3;
                v3X = x2;
                v3Y = y2;
            }
            if ((side!=side1) || (Pow2(diagonal)-2*Pow2(side) == 0))
                return (double.NaN, double.NaN);
            var middleX = Math.Abs(v1X - v2X) / 2;
            var middleY = Math.Abs(v1Y - v2Y) / 2;
            var vectorX = middleX - v3X;
            var vectorY = middleY - v3Y;
            return (middleX + vectorX, middleY + vectorY);
        }

        public static double LineLength(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Pow2(x2 - x1) + Pow2(y2 - y1));
        }

        public static double Pow2(double x) => x * x;

        public static double Max(params double[] p)
        {
            Array.Sort(p);
            return p[p.Length - 1];
        }

    }
}
