using System.Collections.Generic;
using System.Text;

namespace AlgorithmsData
{
    public class FibMatrix
    {
        public int[,] Values;

        public FibMatrix(int[,] values)
        {
            Values = values;
        }

        public FibMatrix Multiply(FibMatrix bm)
        {
            var r = new int[2, 2];
            var a = Values;
            var b = bm.Values;
            r[0, 0] = a[0, 0] * b[0, 0] + a[0, 1] * b[1, 0];
            r[0, 1] = a[0, 0] * b[0, 1] + a[0, 1] * b[1, 1];
            r[1, 0] = a[1, 0] * b[0, 0] + a[1, 1] * b[1, 0];
            r[1, 1] = a[1, 1] * b[1, 1] + a[1, 0] * b[0, 1];
            return new FibMatrix(r);
        }

        public static List<FibMatrix> Generate2PowerList(int maxPower)
        {
            var m = MatrixFibonacciSequence.BasicMatrix;
            var r = new List<FibMatrix>();
            r.Add(m);
            for (var i = 1; i <= maxPower; i++)
            {
                m = m.Multiply(m);
                r.Add(m);
            }
            return r;
        }
    }
    
    public static class MatrixFibonacciSequence
    {
        public static FibMatrix BasicMatrix = new FibMatrix(new int[,] {{1, 1}, {1, 0}});
        public static FibMatrix MatrixOne = new FibMatrix(new int[,] {{1, 0}, {0, 1}});
        
        public static int F(int k)
        {
            k -= 1;
            var b = ToBinary(k);
            var l = FibMatrix.Generate2PowerList(b.Length - 1);
            var r = MatrixOne;
            for (var i = 0; i < b.Length; i++)
                if (b[i] == '1')
                    r = r.Multiply(l[i]);
            return r.Values[0, 0];
        }

        public static string ToBinary(int a)
        {
            var r = new StringBuilder("");
            while (a > 1)
            {
                r.Append(a % 2);
                a /= 2;
            }

            r.Append(1);
            return r.ToString();
        }
    }
}