using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Database.General;

namespace Database
{
	public static class Article
	{
		public static IAsyncEnumerable<Entity> SearchByName(string name, int offset = 0, int number = -1) =>
			Select("articles_with_tags", $"name LIKE '%{name}%'", offset, number/*, "date"*/);

		// /// Returns the link to article, that was passed as parameter!
		// public static Entity ToArticleWithTags(Entity article)
		// {
		// 	var tags = Select("tags_article", $"article_id={article.Values["id"]}");
		// 	article.Values["tags"] = tags;
		// 	return article;
		// }
		
		//public static IEnumerable<Entity> GetArticles()

		public static IAsyncEnumerable<Entity> Get(int offset = 0, int number = -1) =>
			Select("articles_with_tags", offset, number);
		
		// public static IEnumerable<Entity>

		public static async Task<Entity> GetById(int id)
			=> await SelectById("articles_with_tags", id);

		public static IAsyncEnumerable<Entity> SearchByNameAndTag(string name, string tag, int offset = 0,
			int number = -1) => Select($"select_articles_by_name_and_tag('%{name}%', '{tag}')", offset, number);

		public static IAsyncEnumerable<Entity> GetFavoriteArticles(Entity user)
			=> user.GetManyToManyEntities("favorite_articles");

		public static void AddFavoriteArticle(int userId, int articleId)
		{
			var entity = new Entity("favorite_articles");
			entity["user_id"] = userId;
			entity["article_id"] = articleId;
			entity.Insert();
		}
		
		public static void RemoveFavoriteArticle(int userId, int articleId)
		{
			var entity = new Entity("favorite_articles");
			entity["user_id"] = userId;
			entity["article_id"] = articleId;
			entity.Delete();
		}

		public static async Task<bool> IsFavorite(int userId, int articleId)
			=> await Select("favorite_articles", $"user_id={userId} AND article_id={articleId}", 0, 1).AnyAsync();
	}
}