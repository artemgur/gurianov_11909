using System;
using System.Collections.Generic;
using System.Linq;

namespace PostgresADO
{
	public static class Utilities
	{
		public static void WriteSelectResult(IEnumerable<object[]> input)
		{
			foreach (var x in input)
			{
				foreach (var y in x)
				{
					Console.Write(y);
					Console.Write(" ");
				}
				Console.WriteLine();
			}
		}

		public static void WriteIEnumerable(IEnumerable<object> input)
		{
			foreach (var x in input)
				Console.WriteLine(x);
		}

		public static bool ContainsOneOf(this string str, IEnumerable<string> strings) =>
			strings.Any(str.Contains);
	}
}