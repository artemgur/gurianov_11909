using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace HttpListener
{
	public static class PageContent
	{
		private static readonly Regex urlRegex = new Regex(@"http://localhost:8080/(.*)");//TODO add ^ and $
		public static readonly DateTime CookieExpires = DateTime.Parse("25 Dec 2037 23:59:00 GMT");
		private const string CookieName = "colorCookie";
		
		public static string GetFileName(HttpListenerRequest request, HttpListenerResponse response, string query)
		{
			string fileName;
			switch (query)
			{
				case "":
					fileName = "WebPagesFiles\\main.html";
					ManageCookies(request, response);
					break;
				case "calculator":
					fileName = "WebPagesFiles\\calculator.html";
					break;
				case "students":
					fileName = "WebPagesFiles\\students.html";
					break;
				case "table":
					fileName = "WebPagesFiles\\table.html";
					break;
				case "about":
					fileName = "WebPagesFiles\\about_me.html";
					break;
				case "secret":
					if (SessionManager.IsUserLoggedIn(request))
						fileName = "WebPagesFiles\\secret.html";
					else
						fileName = "WebPagesFiles\\404.html";
					break;
				default:
					var path = "WebPagesFiles\\" + query;
					if (File.Exists(path))
						fileName = path;
					//contentType = GetContentType(fileName);
					else
						fileName = "WebPagesFiles\\404.html";
					break;
			}
			//return File.ReadAllText(fileName);
			return fileName;
		}

		private static void ManageCookies(HttpListenerRequest request, HttpListenerResponse response)
		{
			if (request.Cookies[CookieName] == null)
			{
				var cookie = new Cookie(CookieName, "#ffffff");
				cookie.Expires = CookieExpires;
				//Console.WriteLine(cookieExpires.Year);
				response.AppendCookie(cookie);
			}
		}

		public static string GetContentType(string filename)
		{
			var extension = Path.GetExtension(filename);
			switch (extension)
			{
				case ".js":
					return "text/javascript";
				case ".html":
					return "text/html";
				case ".css":
					return "text/css";
				case ".jpg":
					return "image/jpg";
				default:
					return "text/html";
			}
		}

		public static int HandlePost(HttpListenerRequest request)
		{
			var url = request.Url.ToString();
			var finalPart = urlRegex.Match(url).Groups[1].Value;
			if (finalPart == "save_table")
			{
				var fileStream = File.Open("WebPagesFiles\\students_data.json", FileMode.Create);
				request.InputStream.CopyTo(fileStream);
				request.InputStream.Close();
				fileStream.Close();
				return 200;
			}
			if (finalPart == "login")
			{
				string password;
				using (var reader = new StreamReader(request.InputStream))
				{
					password = reader.ReadToEnd();
				}
				if (password == "password")
				{
					SessionManager.MakeUserLoggedIn(request);
					return 200;
				}
			}
			return 400;
		}
		
		
		public static void HandleGet(HttpListenerRequest request, HttpListenerResponse response)
		{
			var url = request.Url.ToString();
			var query = urlRegex.Match(url).Groups[1].Value;
			if (query == "how_many_visitors")
			{
				SendVisitorsNumber(response);
				return;
			}
			var fileName = PageContent.GetFileName(request, response, query);
			var fileStream = File.OpenRead(fileName);
			response.ContentLength64 = fileStream.Length;
			response.ContentType = PageContent.GetContentType(fileName);
			var sw = response.OutputStream;
			fileStream.CopyTo(sw);
			fileStream.Close();
			sw.Close();
		}

		private static void SendVisitorsNumber(HttpListenerResponse response)
		{
			response.ContentType = "text/plain";
			var sw = response.OutputStream;
			var content = Encoding.UTF8.GetBytes(Program.VisitorsCount.ToString());
			sw.Write(content, 0, content.Length);
			sw.Close();
		}
	}
}