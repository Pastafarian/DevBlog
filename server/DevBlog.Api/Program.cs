using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using DevBlog.Api.Helpers;
using DevBlog.Application.Logging;
using System;

namespace DevBlog.Api
{
	public class Program
	{
		public static int Main(string[] args)
        {
            var (appSettings, loggerSettings) = ConfigurationHelper.GetSettings();

            Log.Logger = SerilogBuilder.Build<Program>(appSettings, loggerSettings);

			try
			{
				Log.Information("Starting web host");
				CreateHostBuilder(args).Build().Run();
				return 0;
			}
			catch (Exception ex)
			{
				Log.Information("Exception Here");
				Log.Fatal(ex, "Host terminated unexpectedly");
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();

				}).UseSerilog();
	}
}
