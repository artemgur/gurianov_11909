using System;

namespace PostgresADO
{
	public class ColumnInfo
	{
		public readonly string Name;
		public readonly Type Type;

		public ColumnInfo(string name, Type type)
		{
			Name = name;
			Type = type;
		}
	}
}