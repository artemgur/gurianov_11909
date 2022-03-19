using System;
using System.Collections.Generic;
using System.Text;

namespace SeminarTasks
{
    class Branches7
    {
        static public int LowerRatingToY(int n, double x, double y)
        {
            var e = 0.05 - double.Epsilon;
            x += e;
            var r = n * (x - y - e) / (y + e - 1);
            if (r < 0)
                return -1;
            return (int)Math.Ceiling(r);
        }
    }
}
