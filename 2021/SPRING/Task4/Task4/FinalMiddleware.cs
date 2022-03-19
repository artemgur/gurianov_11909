using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Task4
{
    public class FinalMiddleware
    {
        private readonly RequestDelegate next;
		
        public FinalMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var mediaType = new MediaTypeHeaderValue("text/html");
            mediaType.Encoding = Encoding.UTF8;
            context.Response.ContentType = mediaType.ToString();
            await context.Response.WriteAsync((string)context.Items["output"] + "<br>" + (string)context.Items["counterInfo"]);
        }
    }
}