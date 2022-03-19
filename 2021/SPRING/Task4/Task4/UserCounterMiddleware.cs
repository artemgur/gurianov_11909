using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Task4
{
    public class UserCounterMiddleware
    {
        private static int userCounter = 0;
        
        private readonly RequestDelegate next;
		
        public UserCounterMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
		
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies["counted"] == "true")
            {
                context.Items["counterInfo"] = $"Мы Вас уже посчитали. Пока на сайте было {userCounter} пользователей";
                await next.Invoke(context);
                return;
            }
            if ((string) context.Items["isUser"] == "true")
            {
                userCounter += 1;
                context.Response.Cookies.Append("counted", "true");
                context.Items["counterInfo"] = $"Вы - пользователь номер {userCounter}";
            }
            context.Items["counterInfo"] = $"Пока на сайте было {userCounter} пользователей";
            await next.Invoke(context);
        }
    }
}