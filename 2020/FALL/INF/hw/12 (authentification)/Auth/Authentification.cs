using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using static Database.General;

namespace Auth
{
	public static class Authentication
	{
		//static Dictionary<string, string> authUsers = new Dictionary<string, string>();
		//static byte[] salt = new byte[128 / 8];
		public const string UsersTableName = "users";

		public static void Authenticate(HttpListenerContext context)
		{
			var request = context.Request;
			if (request.HttpMethod != "POST")
				throw new InvalidOperationException();
			string error = "";
			bool justAuth = false;
			string token = "";
			HttpListenerResponse response = context.Response;
			if (context.Request.HttpMethod == "POST")
			{
				// System.IO.Stream body = context.Request.InputStream;
				// System.Text.Encoding encoding = context.Request.ContentEncoding;
				// System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
				// string rawData = reader.ReadToEnd();
				// Dictionary<string, string> postParams = new Dictionary<string, string>();
				// string[] rawParams = rawData.Split('&');
				// foreach (string param in rawParams)
				// {
				// 	string[] kvPair = param.Split('=');
				// 	string key = kvPair[0];
				// 	string value = HttpUtility.UrlDecode(kvPair[1]);
				// 	postParams.Add(key, value);
				// }
				var login = request.Headers["login"];
				var password = request.Headers["password"];
				// if (postParams["login"] == "admin" && GenerateHash(postParams["password"], salt) == GenerateHash("admin", salt))
				// {
				// 	token = GenerateHash(postParams["login"] + "AuthProgram", salt);
				// 	context.Response.AppendCookie(new Cookie("authToken", token));
				// 	authUsers.Add(token, postParams["login"]);
				// 	justAuth = true;
				// }
				// else
				// {
				// 	error = "Неверный логин или пароль!";
				// }
				var user = Select(UsersTableName, "login = " + login).Single();
				if (GenerateHash(password, (byte[]) user.Values["salt"]) == (string) user.Values["password"])
				{
					//TODO
				}
			}
			//
			// string responseStr;
			// if (justAuth || (context.Request.Cookies["authToken"] != null && authUsers.ContainsKey(context.Request.Cookies["authToken"].Value)))
			// {
			// 	responseStr = File.ReadAllText("html/auth.html");
			// 	responseStr = responseStr.Replace("<!--userName-->", authUsers[justAuth ? token : context.Request.Cookies["authToken"].Value]);
			// }
			// else
			// {
			// 	responseStr = File.ReadAllText("html/index.html");
			// 	responseStr = responseStr.Replace("<!--Error-->", error);
			// }
			// byte[] buffer = Encoding.UTF8.GetBytes(responseStr);
			// response.ContentLength64 = buffer.Length;
			// response.OutputStream.Write(buffer, 0, buffer.Length);
			// response.OutputStream.Close();	
		}

		public static void Register(HttpListenerContext context)
		{
			var request = context.Request;
			if (request.HttpMethod != "POST")
				throw new InvalidOperationException();
			var login = request.Headers["login"];
			var password = request.Headers["password"];
			var salt = new byte[128 / 8];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
		}
		
		private static string GenerateHash(string password, byte[] salt)
		{
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 256 / 8));
		}
	}
}