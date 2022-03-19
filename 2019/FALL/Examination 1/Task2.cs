using System;
// +
namespace Examination
{
    class Task2
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            int k = 0;
            int maxK = 0;
            while (n > 1)
            {
                if (n % 2 == 0)
                {
                    k++;
                    if (k > maxK)
                        maxK = k;
                }
                else
                    k = 0;
                n = n / 2;
            }
            Console.WriteLine(maxK.ToString());
        }
    }
}
