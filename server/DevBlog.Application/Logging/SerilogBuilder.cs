using Serilog;
using Serilog.Events;
using System;
using Serilog.Sinks.GoogleCloudLogging;
using DevBlog.Application.Factories;
using DevBlog.Application.Settings;

namespace DevBlog.Application.Logging
{
	public static class SerilogBuilder
	{
		public static Serilog.ILogger Build<TImplementingType>(AppSettings appSettings, LoggerSettings loggerSettings)
        {
            return appSettings.IsProduction ? ProductionLogging<TImplementingType>(appSettings, loggerSettings) : DevelopmentLogging<TImplementingType>(loggerSettings);
        }

        private static Serilog.ILogger DevelopmentLogging<TImplementingType>(LoggerSettings loggerSettings)
        {
            return new LoggerConfiguration()
                .WriteTo.File(loggerSettings.LogFileLocation, rollingInterval: RollingInterval.Day, retainedFileCountLimit: null, shared: true)
                .MinimumLevel.Is(MapLogEventLevel(loggerSettings.LogEventLevel))
                .Enrich.FromLogContext()
                .WriteTo.Seq(loggerSettings.SeqUrl)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .CreateLogger()
                .ForContext(typeof(TImplementingType));
		}

        private static Serilog.ILogger ProductionLogging<TImplementingType>(AppSettings appSettings, LoggerSettings loggerSettings)
        {
			var googleCredentialJson = GoogleCredentialFactory.GetGoogleCredentialJson();

            var config = new GoogleCloudLoggingSinkOptions { ProjectId = appSettings.GcpProjectId, UseJsonOutput = true, GoogleCredentialJson = googleCredentialJson };

            return new LoggerConfiguration()
                .MinimumLevel.Is(MapLogEventLevel(loggerSettings.LogEventLevel))
                .Enrich.FromLogContext()
                .WriteTo.GoogleCloudLogging(config)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .CreateLogger()
                .ForContext(typeof(TImplementingType));
		}

        private static LogEventLevel MapLogEventLevel(LoggingEventType type)
		{
			return type switch
			{
				LoggingEventType.Verbose => LogEventLevel.Verbose,
				LoggingEventType.Debug => LogEventLevel.Debug,
				LoggingEventType.Information => LogEventLevel.Information,
				LoggingEventType.Warning => LogEventLevel.Warning,
				LoggingEventType.Error => LogEventLevel.Error,
				LoggingEventType.Fatal => LogEventLevel.Fatal,
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
			};
		}
	}
}