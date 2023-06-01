using Microsoft.Extensions.DependencyInjection;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class LoggingRegistration
	{
		public static void RegisterLogging(this IServiceCollection services)
        {
            //services.AddSingleton(typeof(ILogger<>), typeof(Application.Logging.Logger<>));
            //services.AddSingleton(typeof(Application.Logging.ILogger<>), typeof(Application.Logging.Logger<>));

            //services.AddSingleton(typeof(ILogger), typeof(Application.Logging.Logger));
            //services.AddSingleton(typeof(Application.Logging.ILogger), typeof(Application.Logging.Logger));
        }
	}
}