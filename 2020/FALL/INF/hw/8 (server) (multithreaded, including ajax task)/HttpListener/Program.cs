using System;
using System.IO;
using System.Threading.Tasks;

namespace HttpListener
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			var listener = new System.Net.HttpListener();
			listener.Prefixes.Add("http://localhost:8080/");
			listener.Start();
			Console.WriteLine("Сервер начал прослушивание порта 8080");

			while (true)
			{
				var context = await listener.GetContextAsync();
				Task.Run(() =>
				{
					var request = context.Request;
					var response = context.Response;
					if (request.HttpMethod == "POST")
					{
						response.StatusCode = PageContent.HandlePost(request);
					}
					else
					{
						var fileName = PageContent.GetFileName(request.Url.ToString());
						var fileStream = File.OpenRead(fileName);
						response.ContentLength64 = fileStream.Length;
						response.ContentType = PageContent.GetContentType(fileName);
						var sw = response.OutputStream;
						fileStream.CopyTo(sw);
						fileStream.Close();
						sw.Close();
					}
				});
			}
		}
	}
}
