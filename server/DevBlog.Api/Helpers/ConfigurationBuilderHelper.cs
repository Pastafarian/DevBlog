
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DevBlog.Api.Helpers
{
	public static class ConfigurationHelper
	{
        public static IConfiguration Build()
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