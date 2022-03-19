using System;
using System.Globalization;
using System.Text;
using static System.Math;
using static Sem1.Utilities;

namespace Sem1
{
    public struct Segment
    {
        public readonly double X1;
        public readonly double Y1;
        public readonly double X2;
        public readonly double Y2;
        public readonly double Length;
        public readonly double Angle;

        public Segment(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Length = Sqrt(Pow2(X2 - X1) + Pow2(Y2 - Y1));
            Angle = Atan2(y2 - y1, x2 - x1);
        }

        public static Segment Parse(string s)
        {
            var l = s.Split();
            return new Segment(double.Parse(l[0], CultureInfo.InvariantCulture), double.Parse(l[1], CultureInfo.InvariantCulture), double.Parse(l[2], CultureInfo.InvariantCulture), double.Parse(l[3], CultureInfo.InvariantCulture));
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(X1);
            builder.Append(' ');
            builder.Append(Y1);
            builder.Append(' ');
            builder.Append(X2);
            builder.Append(' ');
            builder.Append(Y2);
            return builder.ToString();
        }
    }
}