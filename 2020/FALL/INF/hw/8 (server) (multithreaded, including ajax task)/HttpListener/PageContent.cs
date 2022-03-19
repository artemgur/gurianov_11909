using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace HttpListener
{
	public static class PageContent
	{
		private static Regex urlRegex = new Regex(@"http://localhost:8080/(.*)");//TODO add ^ and $
		
		public static string GetFileName(string url)
		{
			//Console.WriteLine(urlRegex.Match(url).Groups[1].Value);
			string fileName;
			//var contentType = "text/html";
			switch (urlRegex.Match(url).Groups[1].Value)
			{
				case "":
					fileName = "WebPagesFiles\\main.html";
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
				
				default:
					var path = "WebPagesFiles\\" + urlRegex.Match(url).Groups[1].Value;
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
			if (finalPart != "save_table")
				return 400;
			var fileStream = File.Open("WebPagesFiles\\students_data.json", FileMode.Create);
			 request.InputStream.CopyTo(fileStream);
			 request.InputStream.Close();
			 fileStream.Close();
			 //request.InputStream.
			//fileStream.Close();
			// var reader = new StreamReader(request.InputStream);
			// File.WriteAllText("WebPagesFiles\\students_data.json", reader.ReadToEnd());
			return 200;
		}
	}
}