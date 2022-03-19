using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RegexTask
{
	public static class Utilities
	{
		public static int[] GenerateNumbers(int count, Regex regex, out int numbersGenerated)
		{
			var i = 0;
			numbersGenerated = 0;
			var random = new Random();
			var result = new int[count];
			while (i < count)
			{
				var n = random.Next(1, int.MaxValue - 1);
				numbersGenerated++;
				if (!regex.IsMatch(n.ToString())) continue;
				result[i] = n;
				i++;
			}
			return result;
		}
	}
}