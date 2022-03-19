using System;

namespace AlgorithmsData
{
    public static class RecursiveFibonacciSequence
    {
        public static int F(int k)
        {
            if (k < 1)
                throw new ArgumentException();
            if (k == 1 || k == 2)
                return 1;
            return F(k - 1) + F(k - 2);
        }
    }
}