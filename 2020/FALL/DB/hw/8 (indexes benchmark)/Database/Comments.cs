using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database
{
	public static class Comments
	{
		public static async Task<Dictionary<bool, List<Entity>>> Get(Entity debate)
		{
			return (await debate.GetOneToManyChildren("debate_id", "comments").ToListAsync()).GroupBy(x => (bool) x["opinion"])
				.Select(x => x.OrderBy(x => (DateTime)x["date"]).ToList()).ToDictionary(x => (bool)x[0]["opinion"], x => x);
		}


		public static void Save(int id, string debateId, string text, string opinion)
		{
			var entity = new Entity("comments");
			entity["debate_id"] = debateId;
			entity["text"] = text;
			entity["user_id"] = id;
			entity["opinion"] = opinion;
			entity["date"] = DateTime.UtcNow;
			entity.Insert();
		}
	}
}