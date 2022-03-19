	

using System;
using System.Collections.Generic;

namespace AlgorithmsData
{
	public class Fraction
	{
		//TODO properties
		private int n;
		private int d;
		internal Fraction next;


		public Fraction(int n, int d, Fraction next)
		{
			this.N = n;
			this.D = d;
			this.next = next;
		}

		public int D
		{
			get => d;
			set => d = SetD(value);
		}

		public int N
		{
			get => n;
			set => n = value;
		}

		private int SetD(int value)
		{
			if (value == 0)
				throw new ArgumentException();
			if (value < 0)
			{
				value = -value;
				N = -N;
			}
			return value;
		}

		public void Simplify()
		{
			var k = GCD(n, d);
			n = n / k;
			d = d / k;
		}

		public static int GCD(int a, int b)
		{
			while (a > 0 && b > 0)
			{
				if (a > b)
					a = a % b;
				else
					b = b % a;
			}
			return a + b;
		}
	}

	public static class FareySequence
	{

		public static List<Fraction> GetFareySequence(int n)
		{
			if (n < 1)
				throw new ArgumentException();
			var start = new Fraction(0, 1, new Fraction(1, 1, null));
			Fraction f, x;
			int num, den;
			for (var i = 1; i <= n; i++)
			{
				f = start;
				while (f.N != 1 || f.D != 1)
				{
					num = f.N + f.next.N;
					den = f.D + f.next.D;
					x = new Fraction(num, den, f.next);
					x.Simplify();
					if (x.D <= i)
						f.next = x;
					f = f.next;
				}
			}
			return CreateArray(start);
		}

		private static List<Fraction> CreateArray(Fraction start)
		{
			var r = new List<Fraction>();
			var f = start;
			while (f != null)
			{
				r.Add(f);
				f = f.next;
			}
			return r;
		}
	}
}
