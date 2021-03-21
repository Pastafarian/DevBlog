using System;
using DevBlog.Application.Settings;

namespace DevBlog.Application.Logging
{
	public class Logger<TImplementingType> : ILogger
	{
		private readonly Serilog.ILogger logger;

		public Logger(AppSettings appSettings, LoggerSettings loggerSettings)
		{
			ValidateSettings(loggerSettings);
			logger = SerilogBuilder.Build<TImplementingType>(appSettings, loggerSettings);
		}

		public void Debug(string messageTemplate)
		{
			logger.Debug(messageTemplate);
		}

		public void Debug<T>(string messageTemplate, T propertyValue)
		{
			logger.Debug(messageTemplate, propertyValue);
		}

		public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			logger.Debug(messageTemplate, propertyValue0, propertyValue1);
		}

		public void Error(string messageTemplate)
		{
			logger.Error(messageTemplate);
		}

		public void Error<T>(string messageTemplate, T propertyValue)
		{
			logger.Error(messageTemplate, propertyValue);
		}

		public void Error(Exception exception, string messageTemplate)
		{
			logger.Error(exception, messageTemplate);
		}

		public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
		{
			logger.Error(exception, messageTemplate, propertyValue);
		}

		public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		public void Fatal(string messageTemplate)
		{
			logger.Fatal(messageTemplate);
		}

		public void Information(string messageTemplate)
		{
			logger.Information(messageTemplate);
		}

		public void Information<T>(string messageTemplate, T propertyValue)
		{
			logger.Information(messageTemplate, propertyValue);
		}

		public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			logger.Information(messageTemplate, propertyValue0, propertyValue1);
		}

		public void Verbose(string messageTemplate)
		{
			logger.Verbose(messageTemplate);
		}

		public void Verbose<T>(string messageTemplate, T propertyValue)
		{
			logger.Verbose(messageTemplate, propertyValue);
		}

		public void Warning(string messageTemplate)
		{
			logger.Warning(messageTemplate);
		}

		public void Warning<T>(string messageTemplate, T propertyValue)
		{
			logger.Warning(messageTemplate, propertyValue);
		}

		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
		private static void ValidateSettings(LoggerSettings settings)
		{
			if (settings == null)
				throw new Exception("LoggerSettings parameter cannot be null");
			if (string.IsNullOrWhiteSpace(settings.LogFileLocation))
				throw new Exception("LogFileLocation cannot be empty");
		}
	}
}

