using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Database
{
	//IMPORTANT! Primary key should be called "id" (without quotes). That probably could be changed, but it'd make code more complex
	public static class General
	{
		private static string connectionString;

		//Select with optional condition, offset and rows number
		public static async IAsyncEnumerable<Entity> Select(string tableName, string condition = null, int offset = 0, int number = -1/*, string orderBy = null*/)
		{
			//TODO check if condition valid
			//var info = EntityInfo.EntityKeys[tableName];
			//var query =  + tableName;
			Utilities.CheckIfTableNameValid(tableName);
			var builder = new StringBuilder("SELECT * FROM ");
			builder.Append(tableName);
			if (condition != null)
			{
				Utilities.CheckIfQueryValid(condition);
				builder.Append(" WHERE ");
				builder.Append(condition);
				//query += "WHERE " + condition;
			}
			// if (orderBy != null)
			// {
			// 	builder.Append(" ORDER BY ");
			// 	builder.Append(orderBy);//TODO fix sql injection
			// 	builder.Append(" DESC");//TODO add possibility for asc?
			// }
			if (offset != 0)
			{
				builder.Append(" OFFSET ");
				builder.Append(offset);
				builder.Append(" ROWS");
			}
			if (number != -1)
			{
				builder.Append(" FETCH FIRST ");
				builder.Append(number);
				builder.Append(" ROWS ONLY");
			}
			await using var connection = new NpgsqlConnection(connectionString);
			await connection.OpenAsync();
			await using var command = new NpgsqlCommand(builder.ToString(), connection);
			await using var reader = await command.ExecuteReaderAsync();
			if (reader.HasRows)
			{
				var columns = GetColumnNames(reader);
				while (await reader.ReadAsync())
				{
					var instance = new Entity(tableName);
					foreach (var key in columns)
						instance.Values[key] = reader[key];
					yield return instance;
				}
			}		
		}

		//Select without condition, but with offset and rows number
		public static IAsyncEnumerable<Entity> Select(string tableName, int offset, int number)
		{
			return Select(tableName, null, offset, number);
		}

		//Inserts entity to table
		public static async Task Insert(this Entity entity)
		{
			//dynamic entity1 = entity;
			//entity1.Abc = 8;
			var builder = new StringBuilder("INSERT INTO ");
			builder.Append(entity.TableName);
			builder.Append(" (");
			var valuesBuilder = new StringBuilder(" VALUES (");
			foreach (var pair in entity.Values)
			{
				builder.Append(pair.Key);
				builder.Append(", ");
				valuesBuilder.Append(pair.Value.ToStringPg());
				valuesBuilder.Append(", ");
			}
			builder.Remove(builder.Length - 2, 2);
			valuesBuilder.Remove(valuesBuilder.Length - 2, 2);
			builder.Append(')');
			valuesBuilder.Append(')');
			builder.Append(valuesBuilder);
			var query = builder.ToString();
			Utilities.CheckIfQueryValid(query);
			await using var connection = new NpgsqlConnection(connectionString);
			await connection.OpenAsync();
			await using var command = new NpgsqlCommand(query, connection);
			await command.ExecuteNonQueryAsync();
		}

		public static async Task<Entity> GetOneToManyParent(this Entity entity, string foreignKey, string foreignTable) =>
			await Select(foreignTable, "id=" + entity.Values[foreignKey].ToStringPg()).SingleAsync();

		public static IAsyncEnumerable<Entity> GetOneToManyChildren(this Entity entity, string foreignKey, string foreignTable) =>
			Select(foreignTable, foreignKey + "=" + entity.Values["id"].ToStringPg());

		public static async IAsyncEnumerable<Entity> GetManyToManyEntities(this Entity entity, string manyToManyTable)
		{
			var relationship = ManyToManyRelationship.Relationships[manyToManyTable];
			var otherTable = relationship.ForeignKeys.Keys.Single(x => x != entity.TableName);
			var otherIds = Select(manyToManyTable,
				relationship.ForeignKeys[entity.TableName] + "=" + entity.Values["id"]).Select(x => x.Values[relationship.ForeignKeys[otherTable]]);
			var condition = "id IN " + await otherIds.ToStringListPg();
			if (condition.Length == 7)//Length of empty condition
				yield break;
			await foreach (var x in Select(otherTable, condition))
				yield return x;
		}

		public static IAsyncEnumerable<Entity> SelectByValue(string tableName, string columnName, object obj, int offset = 0, int number = -1) =>
			Select(tableName, columnName + "=" + obj.ToStringPg(), offset, number);
		
		// public static IAsyncEnumerable<Entity> SelectByValues(string tableName, string columnName, IEnumerable<object> obj, int offset = 0, int number = -1) =>
		// 	Select(tableName, columnName + "=" + obj.ToStringListPg(), offset, number);
		
		public static async Task<Entity> SelectById(string tableName, int pkey) => await Select(tableName, "id=" + pkey).SingleAsync();

		private static string[] GetColumnNames(NpgsqlDataReader reader)
		{
			var fieldCount = reader.FieldCount;
			var result = new string[fieldCount];
			for (var i = 0; i < fieldCount; i++)
				result[i] = reader.GetName(i);
			return result;
		}

		public static async void Delete(this Entity entity)
		{
			var builder = new StringBuilder($"DELETE FROM {entity.TableName} WHERE ");
			foreach (var pair in entity.Values)
			{
				//builder.Append(' ');
				builder.Append(pair.Key);
				builder.Append("=");
				builder.Append(pair.Value.ToStringPg());
				builder.Append("AND ");
			}
			builder.Remove(builder.Length - 4, 4);
			var query = builder.ToString();
			await using var connection = new NpgsqlConnection(connectionString);
			await connection.OpenAsync();
			await using var command = new NpgsqlCommand(query, connection);
			await command.ExecuteNonQueryAsync();
		}

		internal static void InitConnectionString()
		{
			connectionString = File.ReadAllText("ConnectionString.txt");
		}

		public static async Task<string[]> GetEnum(string name)
		{
			Utilities.CheckIfTableNameValid(name);
			var query = $"SELECT enum_range(NULL::{name})";
			await using var connection = new NpgsqlConnection(connectionString);
			await connection.OpenAsync();
			await using var command = new NpgsqlCommand(query, connection);
			await using var reader = await command.ExecuteReaderAsync();
			if (reader.HasRows)
			{
				//var columns = GetColumnNames(reader);
				await reader.ReadAsync();
				return (string[]) reader[0];
			}
			return null;
		}
	}
}