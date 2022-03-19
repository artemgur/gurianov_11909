using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using static Database.General;

namespace Database
{
	public static class User
	{
		public static async Task<Entity> TryLogIn(string username, string password)
		{
			var user = await Get(username);
			if (user == null)
				return null;
			var salt = Convert.FromBase64String((string)user.Values["salt"]);
			var hash = GenerateHash(password, salt);
			if ((string) user.Values["password"] == hash)
				return user;
			return null;
		}

		public static async Task<Entity> TryRegister(string username, string password)
		{
			if (await Get(username) != null)
				return null;
			var salt = GenerateSalt();
			var hash = GenerateHash(password, salt);
			var entity = new Entity("users")
			{
				Values =
				{
					{"username", username},
					{"password", hash},
					{"salt", Convert.ToBase64String(salt)}
				}
			};
			entity.Insert();
			return await Select("users", "username='"+username+"'").SingleAsync();
		}

		public static async Task<Entity> Get(string username) =>
			await Select("users", "username='" + username + "'").SingleOrDefaultAsync();

		private static byte[] GenerateSalt()
		{
			var salt = new byte[16];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(salt);
			return salt;
		}
		
		private static string GenerateHash(string password, byte[] salt)
		{
			return Convert.ToBase64String(KeyDerivation.Pbkdf2
				(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 32));
		}

		public static async Task<Entity> GetById(int id)
			=> await SelectById("users", id);
	}
}