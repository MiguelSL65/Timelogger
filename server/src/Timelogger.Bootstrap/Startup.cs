using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timelogger.Application;
using Timelogger.Bootstrap.Extensions;
using Timelogger.Bootstrap.Middleware;
using Timelogger.Infrastructure;
using Timelogger.Infrastructure.Persistence;

namespace Timelogger.Bootstrap
{
	public class Startup
	{
		private readonly IWebHostEnvironment _environment;
		private readonly IConfigurationRoot _configuration;
		
		private static string _swaggerUrl => "/swagger/v1/swagger.json";
		private static string _swaggerTitle => "Timelogger";

		public Startup(IWebHostEnvironment env)
		{
			_environment = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			
			_configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersFromApiAssembly();
			services.AddTimeloggerDbContext();
			services.AddRepositoriesServices();
			services.ConfigureMediator();
			services.AddSystemClock();
			services.AddSwaggerGen();
			
			// Add framework services.
			services.AddLogging(builder =>
			{
				builder.AddConsole();
				builder.AddDebug();
			});

			services.AddMvc(options => options.EnableEndpointRouting = false);

			if (_environment.IsDevelopment())
			{
				services.AddCors();
			}
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseCors(builder => builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.SetIsOriginAllowed(origin => true)
					.AllowCredentials());

				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint(
					url: _swaggerUrl,
					name: _swaggerTitle));
			}

			app.UseMiddleware<ExceptionMiddlewareHandler>();
			app.UseMvc();
			
			app.UseHttpsRedirection();

			var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
			using var scope = serviceScopeFactory.CreateScope();
			scope.SeedDatabase();
		}
	}
}