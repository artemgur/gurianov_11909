using System;
using System.Text.RegularExpressions;

namespace RegexTask
{
	public static class Regex2
	{
		public static void Main1()
		{
			var regex = new Regex(@"^(((\+?0)|([-+]?([1-9]\d*)))(\.((\d*\(\d*[1-9]\d*\))|(\d*[1-9])))?)$");
			Console.WriteLine(regex.IsMatch(Console.ReadLine()));
		}
	}
}