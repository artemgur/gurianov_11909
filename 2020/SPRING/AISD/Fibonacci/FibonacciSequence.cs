using System;

namespace AlgorithmsData
{
    public static class FibonacciSequence
    {
        public static int F(int k)
        {
            if (k < 1)
                throw new ArgumentException();
            if (k == 1 || k == 2)
                return 1;
            var a = 1;
            var b = 1;
            var c = 1;
            for (var i = 3; i <= k; i++)
            {
                c = b;
                b = a + b;
                a = c;
            }
            return b;
        }
    }
}