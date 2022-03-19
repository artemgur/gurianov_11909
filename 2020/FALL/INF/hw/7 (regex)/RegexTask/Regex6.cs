using System;
using System.Text.RegularExpressions;

namespace RegexTask
{
	public static class Regex6
	{
		public static void Main1()
		{
			var array = Utilities.GenerateNumbers(10, new Regex(@"^((0|2|4|6|8){4,5})$"), out var generated);
			foreach (var x in array)
				Console.Write(x + " ");
			Console.WriteLine();
			Console.WriteLine(generated);
		}
	}
}