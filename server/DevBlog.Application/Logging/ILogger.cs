using System;

namespace DevBlog.Application.Logging
{
	public interface ILogger
	{
		void Debug(string messageTemplate);
		void Debug<T>(string messageTemplate, T propertyValue);
		void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
		void Error(string messageTemplate);
		void Error<T>(string messageTemplate, T propertyValue);
		void Error(Exception exception, string messageTemplate);
		void Error<T>(Exception exception, string messageTemplate, T propertyValue);
		void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
		void Fatal(string messageTemplate);
		void Information(string messageTemplate);
		void Information<T>(string messageTemplate, T propertyValue);
		void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
		void Verbose(string messageTemplate);
		void Verbose<T>(string messageTemplate, T propertyValue);
		void Warning(string messageTemplate);
		void Warning<T>(string messageTemplate, T propertyValue);
	}
}