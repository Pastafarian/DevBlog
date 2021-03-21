using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using DevBlog.Api.Helpers;
using DevBlog.Api.Ioc;
using DevBlog.Api.Middleware;
using DevBlog.Application.Settings;
using DevBlog.Domain;

namespace DevBlog.Api
{
	// Seq: http://localhost:57184/swagger/index.html
	// Swagger: http://localhost:5341/
	public class Startup
	{
		private readonly Container container = IocHelper.BuildContainer();

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		private IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.UseSimpleInjectorAspNetRequestScoping(container);
			services.AddControllers();
		
			services.AddCors(x => x.AddPolicy("Any", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
			services.AddAuthenticationAndAuthorization(Configuration["Auth0:Domain"], Configuration["Auth0:ApiIdentifier"]);
			services.AddResponseCompression();
			services.AddSwagger();

			services.Configure<FormOptions>(x =>
			{
				x.ValueLengthLimit = int.MaxValue;
				x.MultipartBodyLengthLimit = long.MaxValue;
				x.MemoryBufferThreshold = int.MaxValue;
			});

			services.AddDbContext<Context>(options => options.UseNpgsql(Configuration.GetConnectionString("StephenAdamIo")));

			services.AddSimpleInjector(container, options =>
			{
				options.AddAspNetCore().AddControllerActivation();
				options.AddLogging();
			});

			var loggerSettings = Configuration.GetSection("Logging").Get<LoggerSettings>();
			var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

			// We might be able to get rid of this
            services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
			});

			services.AddHttpContextAccessor();
			container.Register(appSettings, loggerSettings);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSimpleInjector(container);

			if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    await context.Response.WriteAsync($"Unable to find end point. Url: {context.Request.GetDisplayUrl()}");
                }
            });

			// We might be able to get rid of this
			//https://stackoverflow.com/questions/52365434/linux-asp-net-core-with-apache-and-reverse-proxy
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

            app.UseRouting();
            app.UseAuthorization();

			app.UseMiddleware<EnableRequestRewindMiddleware>(container);
			app.UseMiddleware<ExceptionHandlerMiddleware>(container);

			app.UseCors("Any");
			app.UseAuthentication();
			app.UseResponseCompression();
			
			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "StephenAdam.dev");
			});

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


			container.Verify();
			
			try
			{
				using (Scope scope = AsyncScopedLifestyle.BeginScope(container))
				{
					container.GetInstance<Context>().Database.Migrate();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
        }
	}
}