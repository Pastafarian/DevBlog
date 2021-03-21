using DevBlog.Application.Logging;

namespace DevBlog.Application.Settings
{
	public class LoggerSettings
	{
		public string LogFileLocation { get; set; }
		public string SeqUrl { get; set; }
		public LoggingEventType LogEventLevel { get; set; }
	}
}