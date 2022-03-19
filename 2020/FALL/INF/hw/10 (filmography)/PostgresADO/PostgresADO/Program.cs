using System;
using Npgsql;
using static PostgresADO.Utilities;

namespace PostgresADO
{
	public static class Program
	{
		public static void Main()
		{
			var connectionString = @"Server=127.0.0.1;Port=5432;Database=filmography;User Id=postgres;Password=postgres;";
			using var connection = new NpgsqlConnection(connectionString);
			connection.Open();
			//Utilities.WriteSelectResult(connection.SelectAllFrom("actors"));
			Console.WriteLine("Введите номер команды. 1 - список таблиц, 2 - содержимое таблицы, 3 - содержимое таблицы с условием, 4 - добавить данные в таблицу");
			var x = int.Parse(Console.ReadLine());
			if (x < 1 || x > 4)
			{
				Console.WriteLine("Такой команды нет");
				return;
			}
			var table = "";
			if (x != 1)
			{
				Console.WriteLine("Введите название таблицы");
				table = Console.ReadLine();
			}
			switch (x)
			{
				case 1:
					WriteIEnumerable(connection.GetTablesList());
					break;
				case 2:
					WriteSelectResult(connection.SelectAllFrom(table));
					break;
				case 3:
					Console.WriteLine("Введите условие");
					var condition = Console.ReadLine();
					WriteSelectResult(connection.SelectAllFromWhere(table, condition));
					break;
				case 4:
					Console.WriteLine("Введите названия столбцов в скобках через запятую");
					var columns = Console.ReadLine();
					Console.WriteLine("Введите значения в скобках через запятую");
					var values = Console.ReadLine();
					connection.InsertInto(table, values, columns);
					break;
			}
		}
	}
}
