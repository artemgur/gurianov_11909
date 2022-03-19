using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RegexTask
{
	public static class Regex8
	{
		public static void Main1()
		{
			var text = File.ReadAllText("wikipedia.html");
			var regex = new Regex(@"(\w+)\.([A-Z|a-z]+)(?=(""|\s))");
			var matches = regex.Matches(text);
			for (var i = 0; i < matches.Count; i++)
			{
				Console.WriteLine("FILENAME: "+matches[i].Groups[1].Value);
				Console.WriteLine("EXTENSION: "+matches[i].Groups[2].Value);
			}
		}
	}
}