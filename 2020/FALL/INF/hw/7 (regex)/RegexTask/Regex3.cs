using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RegexTask
{
	public static class Regex3
	{
		public static void Main1()
		{
			var length = int.Parse(Console.ReadLine());
			var result = new List<int>(length);
			var regex = new Regex(@"^((10)+1?|(01)+0?|1+|0+)$");
			for (var i = 1; i <= length; i++)
				if (regex.IsMatch(Console.ReadLine()))
					result.Add(i);
			foreach (var x in result)
				Console.WriteLine(x);
		}
	}
}