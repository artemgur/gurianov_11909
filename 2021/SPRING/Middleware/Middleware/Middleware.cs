using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware
{
	public class Middleware
	{
		private readonly RequestDelegate next;

		private readonly string formHtml;
		
		public Middleware(RequestDelegate next)
		{
			this.next = next;
			formHtml = File.ReadAllText("form.html");
		}
		
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				var name = context.Request.Query["name"].ToString();
				await next.Invoke(context);
				if (string.IsNullOrEmpty(name))
				{
					context.Response.ContentType = "text/html";
					await context.Response.WriteAsync(formHtml);
				}
				else
					await context.Response.WriteAsync($"Hi, {name}");
			}
			catch (Exception e)
			{
				context.Response.StatusCode = 500;
				Console.WriteLine(e.GetType() + ":" + e.Message + "\n" + e.StackTrace);
			}
		}

	}
}