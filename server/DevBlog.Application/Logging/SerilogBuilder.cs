using Serilog;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace DevBlog.Application.Logging
{
	public static class SerilogBuilder
	{
		public static ILogger Build<TImplementingType>(IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger()
                .ForContext(typeof(TImplementingType));

            return logger;
        }

        public static ILogger Build(IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(x =>
            {
                Debug.Write(x);
                logger.Warning(x);
            });

            return logger;
        }
    }
}