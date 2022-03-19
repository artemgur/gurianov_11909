using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Task4
{
    public class UserMiddleware
    {
        private readonly RequestDelegate next;

        private readonly string formHtml;
		
        public UserMiddleware(RequestDelegate next)
        {
            this.next = next;
            formHtml = File.ReadAllText("form.html");
        }
		
        public async Task InvokeAsync(HttpContext context)
        {
            var name = context.Request.Query["name"].ToString();
            if (string.IsNullOrEmpty(name))
            {
                context.Response.ContentType = "text/html";
                context.Items["output"] = formHtml;
                context.Items["isUser"] = "false";
            }
            else
            {
                context.Items["output"] = $"Привет, {name}";
                context.Items["isUser"] = "true";
            }       
            await next.Invoke(context);
        }

    }
}