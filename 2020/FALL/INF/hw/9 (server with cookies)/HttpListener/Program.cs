using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HttpListener
{
	public static class Program
	{
		private static HashSet<string> visitsLog;

		private static DateTime visitsLogCleanTime;

		private static int visitorsCount;

		public static int VisitorsCount
		{
			get => visitorsCount;
			private set => visitorsCount = value;
		}

		public static async Task Main()
		{
			visitsLogCleanTime = DateTime.Today.AddDays(1);
			visitsLog = new HashSet<string>();
			SessionManager.Initialize();
			var callback = new TimerCallback(SessionManager.SaveSession);
			var timer = new Timer(callback, 0, 0, 10000);
			await Listen();
		}
		
		private static async Task Listen()
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
					try
					{
						var request = context.Request;
						var response = context.Response;
						UpdateVisitLog(request, response);
						SessionManager.ManageSessions(request, response);
						if (request.HttpMethod == "POST")
						{
							response.StatusCode = PageContent.HandlePost(request);
						}
						else
						{
							PageContent.HandleGet(request, response);
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.GetType() + ": " + e.Message + "\n" + e.StackTrace);
					}
				});
			}
		}

		private static void UpdateVisitLog(HttpListenerRequest request, HttpListenerResponse response)
		{
			lock (visitsLog)
			{
				if (DateTime.Now > visitsLogCleanTime)
				{
					visitsLog = new HashSet<string>();
					visitsLogCleanTime = visitsLogCleanTime.AddDays(1);
				}
				var visitInfo = request.RemoteEndPoint?.Address + " " + request.UserAgent;
				if (!visitsLog.Contains(visitInfo))
					visitsLog.Add(visitInfo);
			}
			visitorsCount = visitsLog.Count;
			//response.Headers.Add("UniqueVisitors", visitsLog.Count.ToString());
		}
	}
}
