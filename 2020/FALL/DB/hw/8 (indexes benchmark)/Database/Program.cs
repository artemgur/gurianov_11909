using System;
using System.Linq;
using System.Threading.Tasks;
using static Database.General;

namespace Database
{
	public class Program
	{
		public static async Task Main()
		{
			InitConnectionString();
			var random = new Random();
			for (var i = 0; i < 1000000; i++)
			{
				var entity = new Entity("indexes_test");
				entity["name"] = new string(Enumerable.Repeat(0, 30).Select(x => (char)random.Next('a', 'z' + 1)).ToArray());
				entity["date"] = new DateTime(random.Next(1444, 2022), random.Next(1,13), random.Next(1, 29));
				entity["description"] = new string(Enumerable.Repeat(0, random.Next(10, 31)).Select(x => (char)random.Next('a', 'z' + 1)).ToArray());
				await entity.Insert();
			}
		}
	}
}