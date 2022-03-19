using System;
// +
namespace Examination
{
    class Task3
    {
        static void Main(string[] args)
        {
            double n;
            var nMax = double.MinValue;
            var nMin = double.MaxValue;
            do
            {
                n = double.Parse(Console.ReadLine());
                if (n > nMax)
                    nMax = n;
                if (n < nMin)
                    nMin = n;
            }
            while (n != 0);
            Console.WriteLine(nMin);
            Console.WriteLine(nMax);
        }
    }
}
