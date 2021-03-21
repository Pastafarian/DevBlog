using SimpleInjector;
using DevBlog.Application.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class LoggingRegistration
	{
		public static void Register(Container container)
		{
			container.RegisterConditional(typeof(ILogger),
				c => typeof(Logger<>).MakeGenericType(c.Consumer != null ? c.Consumer.ImplementationType : typeof(Program)), Lifestyle.Singleton, c => true);
		}
	}
}