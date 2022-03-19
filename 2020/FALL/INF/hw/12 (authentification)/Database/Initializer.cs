using System.Collections.Generic;
using static Database.ManyToManyRelationship;

namespace Database
{
	public static class Initializer
	{
		public static void Init()
		{
			InitManyToMany();
			//InitEntityInfo();
		}
		
		// private static void InitEntityInfo()
		// {
		// 	EntityKeys = new Dictionary<string, string[]>
		// 	{
		// 		["articles"] = /*new EntityInfo(*/new[] {"id", "name", "date", "text", "photo"},
		// 			//new[] {typeof(int), typeof(string), typeof(DateTime), typeof(string), typeof(string)}),
		// 		["users"] = /*new EntityInfo(*/new []{"id", "login", "password", "email", "photo"},
		// 			//new []{typeof(int), typeof(string), typeof(string), typeof(string), typeof(string)}),
		// 		["debates"] = /*new EntityInfo(*/new []{"id", "name", "text", "date"}
		// 			//new []{typeof(int), typeof(string), typeof(string), typeof(string), typeof(DateTime)})
		// 	};
		// 	foreach (var manyToMany in Relationships)
		// 		EntityKeys[manyToMany.Key] = manyToMany.Value.ForeignKeys.Values.ToArray();
		// }

		private static void InitManyToMany()
		{
			Relationships = new Dictionary<string, ManyToManyRelationship>
			{
				{"favourite_articles", new ManyToManyRelationship(new Dictionary<string, string>
				{
					{"users", "user_id"},
					{"articles", "article_id"}
				})}, 
				{"debates_users", new ManyToManyRelationship(new Dictionary<string, string>
				{
					{"users", "user_id"},
					{"debates", "debate_id"}
				})}
			};
		}
	}
}