using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RegexTask
{
	public static class Regex9
	{
		public static void Main1()
		{
			var text = File.ReadAllText("wikipedia.html");
			var paramStringRegex = new Regex(@"(?<=\?)([^\\&?""\s]+)=([^\\&?""\s]+)((&|(&amp;))([^\\&?""\s]+)=([^\\&?""\s]+))*(?=(\s|""))");
			var regex = new Regex(@"([^\\&?""\s]+)=([^\\&?""\s]+)");
			var matchesPS = paramStringRegex.Matches(text);
			for (var i = 0; i < matchesPS.Count; i++)
			{
				Console.WriteLine("PARAM STRING: " + matchesPS[i].Value);
				var matches = regex.Matches(matchesPS[i].Value);
				for (var j = 0; j < matches.Count; j++)
				{
					Console.WriteLine("NAME: " + matches[j].Groups[1]);
					Console.WriteLine("VALUE: " + matches[j].Groups[2]);
				}
				Console.WriteLine();
			}
		}
	}
}