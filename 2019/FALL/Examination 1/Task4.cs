using System;

// +- использование статических полей лишнее тут
namespace Examination
{
    class Task4
    {

        static double factorial = 1;
        static double numerator = 0.5;
        static double x;
        static int k;
        static double row;

        static void Main(string[] args)
        {
            x = double.Parse(Console.ReadLine());
            var e = double.Parse(Console.ReadLine());
            var trueResult = Pow2(Math.Cos(x));
            k = 1;
            row = 1;
            while (true)
            {
                row = EvaluateRow();
                if (Math.Abs(row - trueResult) <= e)
                    break;
                k++;
            }
            Console.WriteLine(row);
        }

        static double EvaluateRow()
        {
            factorial *= 2*k * (2*k - 1);
            numerator *= -4 * Pow2(x);
            return row + numerator / factorial;
        }

        static double Pow2(double x)
        {
            return x * x;
        }
    }
}
