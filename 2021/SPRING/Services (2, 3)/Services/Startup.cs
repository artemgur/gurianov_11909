using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Task4;

namespace Services
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<ILogger, ConsoleLogger>();
			services.AddServicesFromConfig("loggingConfig");
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseMiddleware<UserMiddleware>();
			app.UseMiddleware<UserCounterMiddleware>();
			app.UseMiddleware<FinalMiddleware>();

			// app.UseEndpoints(endpoints =>
			// {
			// 	endpoints.MapGet("/", FinalMiddleware.Run);
			// });
		}
	}
}