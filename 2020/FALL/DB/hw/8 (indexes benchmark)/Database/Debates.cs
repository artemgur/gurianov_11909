using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Database.General;

namespace Database
{
	public class Debates
	{
		public static IAsyncEnumerable<Entity> GetUserDebates(Entity user)
			=> user.GetManyToManyEntities("debates_users");
		
		public static IAsyncEnumerable<Entity> Get(int offset = 0, int number = -1) =>
			Select("debates_with_tags", offset, number);
		
		public static async Task<Entity> GetById(int id)
			=> await SelectById("debates_with_tags", id);
		
		public static IAsyncEnumerable<Entity> SearchByName(string name, int offset = 0, int number = -1) =>
			Select("debates_with_tags", $"name LIKE '%{name}%'", offset, number/*, "date"*/);

		public static IAsyncEnumerable<Entity> SearchByNameAndTag(string name, string tag, int offset = 0,
			int number = -1) => Select($"select_debates_by_name_and_tag('%{name}%', '{tag}')", offset, number);

		public static async Task<int> Create(string title, string text, string[] tags, int userId)
		{
			var entity = new Entity("debates");
			entity["name"] = title;
			entity["text"] = text;
			entity["date"] = DateTime.UtcNow;
			entity.Insert();
			var index = (int)(await Select("debates", $"name='{title}' AND date={entity["date"].ToStringPg()}").SingleAsync())["id"];
			var entity1 = new Entity("debates_users");
			entity1["user_id"] = userId;
			entity1["debate_id"] = index;
			entity1.Insert();
			foreach (var tag in tags)
			{
				var entity2 = new Entity("tags_debates");
				entity2["debate_id"] = index;
				entity2["tag"] = tag;
				entity2.Insert();
			}
			return index;
		}
	}
}