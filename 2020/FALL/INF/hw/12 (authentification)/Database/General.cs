using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace Database
{
	//IMPORTANT! Primary key should be called "id" (without quotes). That probably could be changed, but it'd make code more complex
	
	public static class General
	{
		private static string connectionString = @"Server=127.0.0.1;Port=5432;Database=test;User Id=postgres;Password=postgres;";//Filmography for test

		//Select with optional condition, offset and rows number
		public static IEnumerable<Entity> Select(string tableName, string condition = null, int offset = 0, int number = -1/*, string orderBy = null*/)
		{
			//TODO check if condition valid
			//var info = EntityInfo.EntityKeys[tableName];
			//var query =  + tableName;
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
			using var connection = new NpgsqlConnection(connectionString);
			connection.Open();
			using var command = new NpgsqlCommand(builder.ToString());
			using var reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				var columns = GetColumnNames(reader);
				while (reader.Read())
				{
					var instance = new Entity(tableName);
					foreach (var key in columns)
						instance.Values[key] = reader[key];
					yield return instance;
				}
			}		
		}

		//Select without condition, but with offset and rows number
		public static IEnumerable<Entity> Select(string tableName, int offset, int number)
		{
			return Select(tableName, null, offset, number);
		}

		//Inserts entity to table
		public static void Insert(this Entity entity)
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
			using var connection = new NpgsqlConnection(connectionString);
			connection.Open();
			using var command = new NpgsqlCommand(query);
			command.ExecuteNonQuery();
		}

		public static Entity GetOneToManyParent(this Entity entity, string foreignKey, string foreignTable) =>
			Select(foreignTable, "id=" + entity.Values[foreignKey].ToStringPg()).Single();

		public static IEnumerable<Entity> GetOneToManyChildren(this Entity entity, string foreignKey, string foreignTable) =>
			Select(foreignTable, foreignKey + "=" + entity.Values["id"].ToStringPg());

		public static IEnumerable<Entity> GetManyToManyEntities(this Entity entity, string manyToManyTable)
		{
			var relationship = ManyToManyRelationship.Relationships[manyToManyTable];
			var otherTable = relationship.ForeignKeys.Keys.Single(x => x != entity.TableName);
			var otherIds = Select(manyToManyTable,
				relationship.ForeignKeys[entity.TableName] + "=" + entity.Values["id"]).Select(x => x.Values[relationship.ForeignKeys[otherTable]]);
			return Select(otherTable, "id IN " + otherIds.ToStringListPg());
		}

		public static IEnumerable<Entity> SelectByValue(string tableName, string columnName, object obj, int offset = 0, int number = -1) =>
			Select(tableName, columnName + "=" + obj.ToStringPg(), offset, number);
		
		public static IEnumerable<Entity> SelectByValues(string tableName, string columnName, IEnumerable<object> obj, int offset = 0, int number = -1) =>
			Select(tableName, columnName + "=" + obj.ToStringListPg(), offset, number);
		
		public static IEnumerable<Entity> SelectById(string tableName, int pkey) => Select(tableName, "id=" + pkey);

		private static string[] GetColumnNames(NpgsqlDataReader reader)
		{
			var fieldCount = reader.FieldCount;
			var result = new string[fieldCount];
			for (var i = 0; i < fieldCount; i++)
				result[i] = reader.GetName(i);
			return result;
		}
	}
}