using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database
{
	public static class Utilities
	{
		private static readonly string[] ProhibitedStrings = {";", "--", "/*", "*/"};
		
		public static string ToStringPg(this object a)
		{
			if (a is string || a is DateTime)
				return '\'' + a.ToString() + '\'';
			return a.ToString();
		}

		public static string ToStringListPg(this IEnumerable<object> objects)
		{
			var builder = new StringBuilder(" (");
			foreach (var x in objects)
			{
				builder.Append(x.ToStringPg());
				builder.Append(", ");
			}
			builder.Remove(builder.Length - 2, 2);
			builder.Append(')');
			return builder.ToString();
		}

		public static void CheckIfQueryValid(string query)
		{
			if (query.ContainsOneOf(ProhibitedStrings))
				throw new ArgumentException("Possible attempt of SQL injection");
		}
		
		public static bool ContainsOneOf(this string str, IEnumerable<string> strings) =>
			strings.Any(str.Contains);
	}
}