using System.Net;
using System.Text.RegularExpressions;

namespace Auth
{
	public static class RequestHandler
	{
		private static readonly Regex urlRegex = new Regex(@"http://localhost:8080/(.*)");//TODO add ^ and $

		public static void Handle(HttpListenerContext context)
		{
			var request = context.Request;
			var url = request.Url.ToString();
			var finalPart = urlRegex.Match(url).Groups[1].Value;
			switch (finalPart)
			{
				case "":
					SendFile("index.html", context);
					break;
				case "auth":
					if (request.HttpMethod == "GET")
						SendFile("auth.html", context);
					else
						Authentication.Authenticate(context);
					break;
			}
		}

		private static void SendFile(string filename, HttpListenerContext context)
		{
			
		}
	}
}