using System;
using System.Text.RegularExpressions;

namespace RegexTask
{
	public static class Regex11
	{
		public static void Main1()
		{
			Console.WriteLine("1 - opening html tags; 2 - CSSClasses 3 - JS regex literals");
			var variant = int.Parse(Console.ReadLine());
			switch (variant)
			{
				case 1:
					HtmlTags();
					break;
				case 2:
					CSSClasses();
					break;
				case 3:
					JSRegexLiterals();
					break;
			}
		}

		private static void JSRegexLiterals()
		{
			var regex = new Regex(@"^/(.*(//|[^/]))/$");
			Console.WriteLine(regex.IsMatch(Console.ReadLine()));
		}

		private static void CSSClasses()
		{
			var regex = new Regex(@"\.\w+\s*{[^{]*}");
			Console.WriteLine(regex.IsMatch(Console.ReadLine()));
		}

		private static void HtmlTags()
		{
			var regex = new Regex(@"<[^<>]+>");
			Console.WriteLine(regex.IsMatch(Console.ReadLine()));
		}
	}
}