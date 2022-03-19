using System;
using System.Net;
using System.Threading.Tasks;

namespace Auth
{
	public static class Program
	{
		public static async Task Main()
		{
			// using (var rng = RandomNumberGenerator.Create())
			// {
			// 	rng.GetBytes(salt);
			// }
			await Listen();
		}

		private static async Task Listen()
		{
			HttpListener listener = new HttpListener();
			listener.Prefixes.Add("http://localhost:8080/auth/");
			listener.Start();
			Console.WriteLine("WebServer started!");
			while (true)
			{
				HttpListenerContext context = await listener.GetContextAsync();
				Task.Run(() =>
				{
					try
					{
						RequestHandler.Handle(context);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.GetType() + ": " + e.Message + "\n" + e.StackTrace);
					}
				});
			}
			//listener.Stop();
		}
	}
}
