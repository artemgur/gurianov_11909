using System;
// +
namespace Examination
{
    class Task1
    {
        static void Main(string[] args)
        {
            var a = Console.ReadLine();
            var ad = double.Parse(a);
            var b = Console.ReadLine();
            var bd = double.Parse(b);
            var c = Console.ReadLine();
            var cd = double.Parse(c);
            var res = " ";
            a = ChangeFirstNumber(a, ad);
            if (ad == 0)
                b = ChangeFirstNumber(b, bd);
            else
                b = ChangeNumber(b, bd);
            if ((bd != 0) || (ad != 0))
                c = ChangeLastNumber(c, cd);
            if (ad != 0)
                res += a + "x^2";
            if (bd != 0)
                res += b + "x";
            if ((cd != 0) || ((ad == 0) && (bd == 0)))
                res += c;
            Console.WriteLine(res);
        }

        public static string ChangeFirstNumber(string number, double parsedNumber)
        {
            string res = number;
            if (parsedNumber == 1)
                res = "";
            if (parsedNumber == -1)
                res = "-";
            return res;
        }

        public static string ChangeLastNumber(string number, double parsedNumber)
        {
            string res = number;
            if (parsedNumber > 0)
                res = '+' + res;
            return res;
        }

        public static string ChangeNumber(string number, double parsedNumber)
        {
            string res = number;
            if (parsedNumber > 0)
                res = '+' + res;
            if (parsedNumber == 1)
                res = "+";
            if (parsedNumber == -1)
                res = "-";
            return res;
        }
    }
}
