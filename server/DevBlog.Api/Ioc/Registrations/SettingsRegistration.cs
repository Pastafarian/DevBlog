using SimpleInjector;
using DevBlog.Application.Settings;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class SettingsRegistration
	{
		public static void Register(Container container, AppSettings appSettings, LoggerSettings loggerSettings)
		{
			container.Register(() => appSettings, Lifestyle.Singleton);
			container.Register(() => loggerSettings, Lifestyle.Singleton);
		}
	}
}