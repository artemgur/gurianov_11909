using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace HttpListener
{
	public static class SessionManager
	{
		private static Dictionary<Guid, Session> sessions;
		private const string SessionIDCookieName = "sessionID";
		private const string LoggedInName = "loggedIn";
		private const int SessionDurationSeconds = 180;
		private const string SessionFilename = "session.bin";

		public static void Initialize()
		{
			if (!File.Exists(SessionFilename))
			{
				sessions = new Dictionary<Guid, Session>();
				return;
			}
			var stream = File.OpenRead(SessionFilename);
			var formatter = new BinaryFormatter();
			sessions = (Dictionary<Guid, Session>) formatter.Deserialize(stream);
			stream.Close();
		}

		public static void ManageSessions(HttpListenerRequest request, HttpListenerResponse response)
		{
			CleanOldSessionIfExists(request);
			CreateSessionIfNotExists(request, response);
		}

		private static void CreateSessionIfNotExists(HttpListenerRequest request, HttpListenerResponse response)
		{
			if (request.Cookies[SessionIDCookieName] == null)
			{
				var guid = Guid.NewGuid();
				var cookie = new Cookie(SessionIDCookieName, guid.ToString());
				cookie.Expires = PageContent.CookieExpires;
				//Console.WriteLine(cookieExpires.Year);
				response.AppendCookie(cookie);
				lock (sessions)
				{
					sessions.Add(guid, new Session());
				}
			}
			else
			{
				var cookie = request.Cookies[SessionIDCookieName];
				var guid = Guid.Parse(cookie.Value);
				lock (sessions)
				{
					if (!sessions.ContainsKey(guid))
						sessions.Add(guid, new Session());
				}
			}

		}

		private static void CleanOldSessionIfExists(HttpListenerRequest request)
		{
			if (request.Cookies[SessionIDCookieName] != null)
			{
				var cookie = request.Cookies[SessionIDCookieName];
				var guid = Guid.Parse(cookie.Value);
				lock (sessions)
				{
					if (!sessions.ContainsKey(guid))
						return;
					if (sessions[guid].LastVisitTime.AddSeconds(SessionDurationSeconds) < DateTime.Now)
						sessions.Remove(guid);
				}
			}
		}

		public static void MakeUserLoggedIn(HttpListenerRequest request)
		{
			var cookie = request.Cookies[SessionIDCookieName];
			var guid = Guid.Parse(cookie.Value);
			lock (sessions)
			{
				if (!sessions[guid].Data.ContainsKey(LoggedInName) || !(bool)sessions[guid].Data[LoggedInName])
				    sessions[guid].Data.Add(LoggedInName, true);
			}
		}

		public static bool IsUserLoggedIn(HttpListenerRequest request)
		{
			var cookie = request.Cookies[SessionIDCookieName];
			var guid = Guid.Parse(cookie.Value);
			lock (sessions)
			{
				if (!sessions.ContainsKey(guid) || !sessions[guid].Data.ContainsKey(LoggedInName))
					return false;
				return (bool) sessions[guid].Data[LoggedInName];
			}
		}

		public static void SaveSession(object state)
		{
			lock (sessions)
			{
				var stream = File.OpenWrite(SessionFilename);
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, sessions);
				stream.Close();
			}
		}
	}
}