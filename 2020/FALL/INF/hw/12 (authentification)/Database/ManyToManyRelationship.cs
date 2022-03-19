using System.Collections.Generic;

namespace Database
{
	public class ManyToManyRelationship
	{
		public static Dictionary<string, ManyToManyRelationship> Relationships;
		//public readonly string TableName;
		//Key is foreign table name, value is key name
		public readonly Dictionary<string, string> ForeignKeys;

		// static ManyToManyRelationship()
		// {
		// 	Initializer.InitManyToMany();
		// }

		internal ManyToManyRelationship(Dictionary<string, string> foreignKeys)
		{
			ForeignKeys = foreignKeys;
		}
	}
}