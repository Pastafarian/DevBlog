
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using DevBlog.Application.Settings;

namespace DevBlog.Api.Helpers
{
	public static class ConfigurationHelper
	{
        public static (AppSettings appSettings, LoggerSettings loggerSettings) GetSettings()
        {
            var configuration = Build();
            var appSettings = new AppSettings();
            var loggerSettings = new LoggerSettings();

            configuration.GetSection("AppSettings").Bind(appSettings);
            configuration.GetSection("Logging").Bind(loggerSettings);

            return (appSettings, loggerSettings);
        }

		private static IConfiguration Build()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
				.AddEnvironmentVariables()
				.Build();

			return builder;
		}
	}
}