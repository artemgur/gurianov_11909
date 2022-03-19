using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;

namespace PostgresADO
{
	public static class DatabaseExtensions
	{
		//TODO dynamic?
		private static readonly string[] ProhibitedStrings = {";", "--", "/*", "*/"};
		private static readonly Regex TableNameRegex = new Regex(@"^(\w|_|-)+$");//No quoted identifiers, not sure if we need it
		
		public static IEnumerable<string> GetTablesList(this NpgsqlConnection connection)
		{
			using var command = new NpgsqlCommand("SELECT table_name FROM information_schema.tables WHERE table_schema='public' AND table_type='BASE TABLE'", connection);
			using var reader = command.ExecuteReader();
			//var fieldCount = reader.FieldCount;
			if (reader.HasRows)
				while (reader.Read())
					yield return reader.GetValue(0).ToString();
		}

		public static IEnumerable<object[]> SelectAllFrom(this NpgsqlConnection connection, string table)
		{
			CheckIfTableNameValid(table);
			using var command = new NpgsqlCommand("SELECT * FROM " + table, connection);
			//command.Parameters.AddWithValue("p", table);
			using var reader = command.ExecuteReader();
			var fieldCount = reader.FieldCount;
			if (reader.HasRows)
				while (reader.Read())
				{
					var array = new object[fieldCount];
					for (var i = 0; i < fieldCount; i++)
						array[i] = reader.GetValue(i);
					yield return array;
				}
		}

		public static IEnumerable<object[]> SelectAllFromWhere(this NpgsqlConnection connection, string table, string condition)
		{
			CheckIfTableNameValid(table);
			CheckIfQueryValid(condition);
			using var command = new NpgsqlCommand($"SELECT * FROM {table} WHERE {condition}", connection);
			//command.Parameters.AddWithValue("p", table);
			using var reader = command.ExecuteReader();
			var fieldCount = reader.FieldCount;
			if (reader.HasRows)
				while (reader.Read())
				{
					var array = new object[fieldCount];
					for (var i = 0; i < fieldCount; i++)
						array[i] = reader.GetValue(i);
					yield return array;
				}
		}

		private static void CheckIfTableNameValid(string table)
		{
			if (table == null)
				throw new ArgumentNullException(nameof(table));
			if (!TableNameRegex.IsMatch(table))
				throw new ArgumentException("Invalid table name");
		}

		private static void CheckIfQueryValid(string query)
		{
			if (query.ContainsOneOf(ProhibitedStrings))
				throw new ArgumentException("Invalid query");
		}

		public static ColumnInfo[] GetColumnsInfo(this NpgsqlConnection connection, string table)
		{
			CheckIfTableNameValid(table);
			using var command = new NpgsqlCommand($"SELECT * FROM {table} WHERE false", connection);
			//command.Parameters.AddWithValue("p", table);
			using var reader = command.ExecuteReader();
			var fieldCount = reader.FieldCount;
			var result = new ColumnInfo[fieldCount];
			for (var i = 0; i < fieldCount; i++)
			{
				result[i] = new ColumnInfo(reader.GetName(i), reader.GetFieldType(i));
			}
			return result;
		}

		public static void InsertInto(this NpgsqlConnection connection, string table, object[] values, string[] columns = null)
		{
			var columnsString = columns == null ? "" : "(" + string.Join(", ", columns) + ")";
			var valuesString = CreateValueStringFromObjectArray(values);
			connection.InsertInto(table, valuesString, columnsString);
			// CheckIfTableNameValid(table);
			// CheckIfQueryValid(columnsString);
			// CheckIfQueryValid(valuesString);
			// using var command = new NpgsqlCommand($"INSERT INTO {table}{columnsString} VALUES ({valuesString})", connection);
			// command.ExecuteNonQuery();
		}

		public static void InsertInto(this NpgsqlConnection connection, string table, string valuesString,
			string columnsString = null)
		{
			CheckIfQueryValid(columnsString);
			CheckIfQueryValid(valuesString);
			using var command = new NpgsqlCommand($"INSERT INTO {table}{columnsString} VALUES {valuesString}", connection);
			command.ExecuteNonQuery();
		}

		private static string CreateValueStringFromObjectArray(object[] values)
		{
			var builder = new StringBuilder("(");
			for (var index = 0; index < values.Length; index++)
			{
				var value = values[index];
				var type = value.GetType();
				if (type == typeof(string) || type == typeof(DateTime))
				{
					builder.Append('\'');
					builder.Append(value);
					builder.Append('\'');
				}
				else
					builder.Append(value);
				if (index == values.Length - 1)
					break;
				builder.Append(", ");
			}
			builder.Append(")");
			return builder.ToString();
		}
	}
}