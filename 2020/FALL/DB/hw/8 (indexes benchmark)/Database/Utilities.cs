using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Database
{
	public static class Utilities
	{
		private static readonly string[] ProhibitedStrings = {";", "--", "/*", "*/"};
		private static readonly Regex TableNameRegex = new Regex(@"^(\w|_|-)+$");//Quoted identifiers are not supported

		public static string ToStringPg(this object a)
		{
			if (a is string)
				return '\'' + a.ToString() + '\'';
			if (a is DateTime d)
				return $"'{d.Year}-{d.Month}-{d.Day} {d.Hour}:{d.Minute}:{d.Second}'";
			return a.ToString();
		}

		public static async Task<string> ToStringListPg(this IAsyncEnumerable<object> objects)
		{
			var builder = new StringBuilder(" (");
			await foreach (var x in objects)
			{
				builder.Append(x.ToStringPg());
				builder.Append(", ");
			}
			builder.Remove(builder.Length - 2, 2);
			builder.Append(')');
			return builder.ToString();
		}
		
		internal static void CheckIfTableNameValid(string table)
		{
			if (table == null)
				throw new ArgumentNullException(nameof(table));
			if (!TableNameRegex.IsMatch(table))
				throw new ArgumentException("Invalid table name");
		}

		internal static void CheckIfQueryValid(string query)
		{
			if (query.ContainsOneOf(ProhibitedStrings))
				throw new ArgumentException("Possible attempt of SQL injection");
		}
		
		public static bool ContainsOneOf(this string str, IEnumerable<string> strings) =>
			strings.Any(str.Contains);
	}
}